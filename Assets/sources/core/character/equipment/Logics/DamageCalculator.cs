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
        public static void DealDamage(ICharacter attacker, Stat stat, GameObject target, Vector2 point)
        {
            var reciever = target.GetComponentInParent<IAttackable>(false);
            if (reciever == null || reciever.CurrentHitPoint <= 0 || reciever.IsDead)
            {
                attacker.OnAttackMiss(point);
            }
            else if (target.tag == "Grass")
            {
                if (Common.Utils.RandomByProbability(stat.FireRate))
                {
                    reciever.StatusEffectManager.SetFire(10);

                    int damage = (int)(GetAttackDamage(stat, attacker) * stat.FireRate);
                    if (damage != 0)
                    {
                        SendDamage(reciever, damage);
                        _damageDisplay.DisplayDamage(damage, point, attacker is Patapons.Patapon);
                    }
                }
            }
            else
            {
                (int damage, bool isCritical) = GetFinalDamage(attacker, reciever, stat);
                if (reciever is Bosses.Boss boss)
                {
                    damage = (int)(damage * boss.GetBrokenPartMultiplier(target, damage));
                }

                SendDamage(reciever, damage);
                _damageDisplay.DisplayDamage(damage, point, attacker is Patapons.Patapon);

                CheckIfDie(reciever, target);
                attacker.OnAttackHit(point, damage);
            }
        }
        /// <summary>
        /// Gets fire duration based on attacker stats and reciever stats.
        /// </summary>
        /// <param name="senderStat">Stat of attacker.</param>
        /// <param name="recieverStat">Stat of damage taker.</param>
        /// <param name="time">Initial time, before calculated.</param>
        /// <returns>Calculated final time of fire effect duration.</returns>
        public static int GetFireDuration(Stat senderStat, Stat recieverStat, int time)
        {
            //will be implemented later!
            return (int)(time * 0.75f);
        }
        public static void DealDamageFromFireEffect(IAttackable attackable, GameObject targetObject, Transform objectTransform, bool displayDamage = true)
        {
            //--- add fire resistance to fire damage taking!
            var damage = Mathf.Max(1, (int)(attackable.Stat.HitPoint * 0.05f));
            SendDamage(attackable, damage);
            if (displayDamage) _damageDisplay.DisplayDamage(damage, objectTransform.position, false);
            CheckIfDie(attackable, targetObject);
        }
        private static void CheckIfDie(IAttackable target, GameObject targetObject)
        {
            if (target.CurrentHitPoint <= 0)
            {
                foreach (var collider in targetObject.GetComponentsInChildren<Collider2D>()) collider.enabled = false;
                //do destroy action.
                target.Die();
            }
        }
        private static void SendDamage(IAttackable target, int damage)
        {
            target.TakeDamage(damage);
            target.OnDamageTaken?.Invoke((float)target.CurrentHitPoint / target.Stat.HitPoint);
        }
        private static (int damage, bool isCritical) GetFinalDamage(ICharacter attacker, IAttackable reciever, Stat attackerStat)
        {
            var damage = GetAttackDamage(attackerStat, attacker);
            var defence = GetDefence(reciever);
            var critical = GetCritical(attackerStat, reciever);
            return (Mathf.RoundToInt(Mathf.Max(0, damage) * (critical + 1) / Mathf.Max(0.1f, defence)), critical > 0);
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
        private static int GetAttackDamage(Stat stat, ICharacter character) => GetFinalValue(stat.DamageMin, stat.DamageMax, character.GetAttackValueOffset());
        private static int GetDefence(IAttackable attackable) => GetFinalValue(attackable.Stat.DefenceMin, attackable.Stat.DefenceMax, attackable.GetDefenceValueOffset());
        private static int GetFinalValue(int min, int max, float offset) => Mathf.RoundToInt(Mathf.Lerp(min, max, offset));
        private static int GetFinalValue(float min, float max, float offset) => Mathf.RoundToInt(Mathf.Lerp(min, max, offset));
    }
}
