using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Logic
{
    internal static class DamageCalculator
    {
        private static readonly DamageDisplay _damageDisplay = new DamageDisplay();
        /// <summary>
        /// Calculates damage, while BEING ATTACKED. Doesn't calculate status effect damage (like fire).
        /// </summary>
        /// <param name="attacker">Who deals the damage.</param>
        /// <param name="stat">Stat when the weapon is fired. This is necessary, especially for range attack.</param>
        /// <param name="target">Who takes the damage.</param>
        /// <param name="point">The point where the damage hit.</param>
        /// <returns><c>true</c> if found target to deal damage, otherwise <c>false</c>.</returns>
        public static void DealDamage(ICharacter attacker, Stat stat, GameObject target, Vector2 point, bool allowZero = false)
        {
            var receiver = target.GetComponentInParent<IAttackable>(false);
            if (receiver == null || receiver.CurrentHitPoint <= 0 || receiver.IsDead)
            {
                attacker.OnAttackMiss(point);
            }
            else if (target.tag == "Grass")
            {
                var fireRateMultiplier = Map.Weather.WeatherInfo.Current.FireRateMultiplier;
                var probability = CalculateStatusEffect(stat.FireRate, (1 - (float)receiver.CurrentHitPoint / receiver.Stat.HitPoint), fireRateMultiplier);
                if (Common.Utils.RandomByProbability(probability))
                {
                    receiver.StatusEffectManager.SetFire(2 + Mathf.RoundToInt(probability * 10 * fireRateMultiplier));

                    int damage = Mathf.Max(0, (int)(GetAttackDamage(stat, attacker) * stat.FireRate));
                    if (damage != 0)
                    {
                        SendDamage(receiver, damage);
                        _damageDisplay.DisplayDamage(damage, point, attacker is Patapons.Patapon, false);
                    }
                }
            }
            else
            {
                (int damage, bool isCritical) = GetFinalDamage(attacker, receiver, stat);
                damage = Mathf.Max(allowZero ? 0 : 1, damage);
                if (receiver is Bosses.Boss boss)
                {
                    damage = (int)(damage * boss.GetBrokenPartMultiplier(target, damage));
                }

                SendDamage(receiver, damage);
                _damageDisplay.DisplayDamage(damage, point, attacker is Patapons.Patapon, isCritical);


                if (!CheckIfDie(receiver, target))
                {
                    attacker.OnAttackHit(point, damage);

                    //------ status effect
                    if (!receiver.StatusEffectManager.IsOnStatusEffect)
                    {
                        var receiverStat = receiver.Stat;
                        if (!CalculateAndSetStagger(receiver, stat.Stagger))
                        {
                            CalculateAndSetKnockback(receiver, stat.Knockback);
                        }

                        var fireRateMultiplier = Map.Weather.WeatherInfo.Current.FireRateMultiplier;
                        var iceRateMultiplier = Map.Weather.WeatherInfo.Current.IceRateMultiplier;

                        var fireProbability = CalculateStatusEffect(stat.FireRate, receiverStat.FireResistance, 0.5f * fireRateMultiplier);
                        var iceProbability = CalculateStatusEffect(stat.IceRate, receiverStat.IceResistance, 0.5f * iceRateMultiplier);
                        var sleepProbability = CalculateStatusEffect(stat.SleepRate, receiverStat.SleepResistance, 0.5f);

                        var random = Random.Range(0, fireProbability + iceProbability + sleepProbability);

                        if (random < fireProbability)
                        {
                            SetStatusEffect(receiver, StatusEffectType.Fire, fireProbability);
                        }
                        else if (random < fireProbability + iceProbability)
                        {
                            SetStatusEffect(receiver, StatusEffectType.Ice, iceProbability);
                        }
                        else
                        {
                            SetStatusEffect(receiver, StatusEffectType.Sleep, sleepProbability);
                        }
                    }
                    //--- status effect end
                }
            }
        }
        /// <summary>
        /// Gets fire duration based on attacker stats and reciever stats.
        /// </summary>
        /// <param name="senderStat">Stat of attacker.</param>
        /// <param name="recieverStat">Stat of damage taker.</param>
        /// <param name="time">Initial time, before calculated.</param>
        /// <returns>Calculated final time of fire effect duration.</returns>
        public static float GetFireDuration(Stat senderStat, Stat recieverStat, float time)
        {
            return CalculateStatusEffect(senderStat.FireRate, recieverStat.FireRate) * time;
        }
        public static void DealDamageFromFireEffect(IAttackable attackable, GameObject targetObject, Transform objectTransform, bool displayDamage = true)
        {
            //--- add fire resistance to fire damage taking!
            var damage = Mathf.Max(1, (int)(attackable.Stat.HitPoint * 0.05f));
            SendDamage(attackable, damage);
            if (displayDamage) _damageDisplay.DisplayDamage(damage, objectTransform.position, false, false);
            CheckIfDie(attackable, targetObject);
        }
        private static bool CheckIfDie(IAttackable target, GameObject targetObject)
        {
            if (target.CurrentHitPoint <= 0)
            {
                foreach (var collider in targetObject.GetComponentsInChildren<Collider2D>()) collider.enabled = false;
                //do destroy action.
                target.Die();
                return true;
            }
            else return false;
        }
        private static void SendDamage(IAttackable target, int damage)
        {
            target.TakeDamage(damage);
            target.OnDamageTaken?.Invoke((float)target.CurrentHitPoint / target.Stat.HitPoint);
        }
        private static (int damage, bool isCritical) GetFinalDamage(ICharacter attacker, IAttackable receiver, Stat attackerStat)
        {
            var damage = GetAttackDamage(attackerStat, attacker);
            var defence = GetDefence(receiver);
            var critical = GetCritical(attackerStat, receiver);
            var resistances = receiver.AttackTypeResistance;

            //"Elemental damage"
            var fireProbability = Mathf.Max(0, attackerStat.FireRate - receiver.Stat.FireResistance) * resistances.FireMultiplier;
            var iceProbability = Mathf.Max(0, attackerStat.IceRate - receiver.Stat.IceResistance) * resistances.IceMultiplier;
            var thunderProbability = Mathf.Max(0, attackerStat.FireRate - receiver.Stat.FireResistance) * resistances.ThunderMultiplier;

            float elementalMultiplier = 1;
            switch (attacker.ElementalAttackType)
            {
                case Weapons.ElementalAttackType.Fire:
                    elementalMultiplier = Mathf.Max(0.1f, 1 + fireProbability);
                    break;
                case Weapons.ElementalAttackType.Ice:
                    elementalMultiplier = Mathf.Max(0.1f, 1 + iceProbability);
                    break;
                case Weapons.ElementalAttackType.Thunder:
                    elementalMultiplier = Mathf.Max(0.1f, 1 + thunderProbability);
                    break;
            }

            if (defence <= 0)
            {
                //You give Infinity, I'll do this by myself
                throw new System.InvalidOperationException(
                    "defence cannot be zero or less. check stat of " + (receiver as MonoBehaviour)?.name ?? " ...oh what receiver is not MonoBehaviour WTF?");
            }
            //a bit complicated...
            return (Mathf.RoundToInt(
                    Mathf.Max(1,
                        (
                        elementalMultiplier //elemental damage
                        * resistances.GetMultiplier(attacker.AttackType) //attack type multiplier
                        * resistances.GetMultiplier(attacker.ElementalAttackType) //elemental attack type multiplier
                        * Mathf.Max(0.1f, damage) //actual damage
                        * (critical + 1) //critical
                        + (fireProbability + iceProbability + thunderProbability) * (critical + 1) * Mathf.Max(0.1f, damage)
                        )
                        / defence //defence
                    )
                ), critical > 0);
        }
        private static float GetCritical(Stat attackerStat, IAttackable reciever)
        {
            var critChance = Mathf.Max(0, attackerStat.Critical - reciever.Stat.CriticalResistance);
            if (Common.Utils.RandomByProbability(critChance))
            {
                return critChance;
            }
            else return 0;
        }
        public static bool CalculateAndSetStagger(IAttackable receiver, float staggerRate) =>
            CalculateAndSetStatusEffect(receiver, StatusEffectType.Stagger, staggerRate, receiver.Stat.StaggerResistance, 0.1f);
        public static bool CalculateAndSetKnockback(IAttackable receiver, float knockback) =>
            CalculateAndSetStatusEffect(receiver, StatusEffectType.Knockback, knockback, receiver.Stat.KnockbackResistance, 0.1f);
        public static bool CalculateAndSetStatusEffect(IAttackable receiver, StatusEffectType type, float attackRatio, float resistance, float additionalMultiplier = 1) =>
            SetStatusEffect(receiver, type, CalculateStatusEffect(attackRatio, resistance, additionalMultiplier));

        private static bool SetStatusEffect(IAttackable receiver, StatusEffectType type, float probability)
        {
            if (Common.Utils.RandomByProbability(probability))
            {
                switch (type)
                {
                    case StatusEffectType.Fire:
                        receiver.StatusEffectManager.SetFire(2 + probability * 10 * Map.Weather.WeatherInfo.Current.FireRateMultiplier);
                        break;
                    case StatusEffectType.Ice:
                        receiver.StatusEffectManager.SetIce(2 + probability * 10 * Map.Weather.WeatherInfo.Current.IceRateMultiplier);
                        break;
                    case StatusEffectType.Sleep:
                        receiver.StatusEffectManager.SetSleep(4 + probability * 25);
                        break;
                    case StatusEffectType.Stagger:
                        receiver.StatusEffectManager.SetStagger();
                        break;
                    case StatusEffectType.Knockback:
                        receiver.StatusEffectManager.SetKnockback();
                        break;
                    default:
                        return false;
                }
                return true;
            }
            else return false;
        }
        private static float CalculateStatusEffect(float attackRatio, float resistance, float additionalMultiplier = 1)
        {
            if (attackRatio == 0 || resistance == Mathf.Infinity) return 0;
            else return Mathf.Clamp01(attackRatio - resistance) * additionalMultiplier;
        }
        private static int GetAttackDamage(Stat stat, ICharacter character) => GetFinalValue(stat.DamageMin, stat.DamageMax, character.GetAttackValueOffset());
        private static float GetDefence(IAttackable attackable) => GetFinalValue(attackable.Stat.DefenceMin, attackable.Stat.DefenceMax, attackable.GetDefenceValueOffset());
        private static int GetFinalValue(int min, int max, float offset) => Mathf.RoundToInt(Mathf.Lerp(min, max, offset));
        private static float GetFinalValue(float min, float max, float offset) => Mathf.Lerp(min, max, offset);
    }
}
