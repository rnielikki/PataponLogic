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
            if (component == null || component.CurrentHitPoint <= 0)
            {
                attacker.OnAttackMiss(point);
                return;
            }
            var damage = attacker.GetAttackDamage(stat);
            component.TakeDamage(damage);
            _damageDisplay.DisplayDamage(damage, point, attacker is Patapons.Patapon);
            CheckIfDie(component, target);

            attacker.OnAttackHit(point);
        }
        public static void DealDamageFromFireEffect(IAttackable attackable, GameObject targetObject, Transform objectTransform)
        {
            //--- add fire resistance to fire damage taking!
            var damage = 10;
            attackable.TakeDamage(damage);
            _damageDisplay.DisplayDamage(damage, objectTransform.position, false);
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
