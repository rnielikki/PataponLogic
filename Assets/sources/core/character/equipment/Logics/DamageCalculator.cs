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
            var component = target.GetComponentInParent<IAttackable>();
            if (component == null || component.CurrentHitPoint <= 0 || component.IsDead)
            {
                attacker.OnAttackMiss(point);
            }
            else if (target.tag == "Grass")
            {
                if (stat.FireRate > 0)
                {
                    component.StatusEffectManager.SetFire(10);
                    int damage = (int)(attacker.GetAttackDamage(stat) * stat.FireRate);
                    if (damage != 0)
                    {
                        _damageDisplay.DisplayDamage(damage, point, attacker is Patapons.Patapon);
                        component.TakeDamage((int)(damage * 0.1f));
                    }
                }
            }
            else
            {
                var damage = attacker.GetAttackDamage(stat);
                component.TakeDamage(damage);
                _damageDisplay.DisplayDamage(damage, point, attacker is Patapons.Patapon);
                CheckIfDie(component, target);
                attacker.OnAttackHit(point);
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
            attackable.TakeDamage(damage);
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
    }
}
