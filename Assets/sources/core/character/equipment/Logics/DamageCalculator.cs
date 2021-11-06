using UnityEngine;

namespace PataRoad.Core.Character.Equipment.Logic
{
    internal static class DamageCalculator
    {
        private static readonly DamageDisplay _damageDisplay = new DamageDisplay();
        /// <summary>
        /// Calculates damage, while BEING ATTACKED. Doesn't calculate status effect damage (like fire).
        /// </summary>
        /// <param name="attacker">Who deals the damage.</param>
        /// <param name="target">Who takes the damage.</param>
        /// <param name="point">The point where the damage hit.</param>
        /// <returns><c>true</c> if found target to deal damage, otherwise <c>false</c>.</returns>
        public static void DealDamage(ICharacter attacker, GameObject target, Vector2 point)
        {
            var component = target.GetComponentInParent<IAttackable>();
            if (component == null)
            {
                attacker.OnAttackMiss(point);
                return;
            }
            var damage = attacker.GetAttackDamage();
            component.TakeDamage(damage);
            _damageDisplay.DisplayDamage(damage, point);
            if (component.CurrentHitPoint <= 0)
            {
                foreach (var collider in target.GetComponentsInChildren<Collider2D>()) collider.enabled = false;
                //do destroy action.
                component.Die();
            }
            attacker.OnAttackHit(point);
        }
    }
}
