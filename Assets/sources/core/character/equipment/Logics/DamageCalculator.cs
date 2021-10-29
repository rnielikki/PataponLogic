using UnityEngine;

namespace Core.Character.Equipment.Logic
{
    internal static class DamageCalculator
    {
        private static readonly DamageDisplay _damageDisplay = new DamageDisplay();
        public static void DealDamage(ICharacter attacker, GameObject target, Vector2 point)
        {
            var component = target.GetComponent<IAttackable>();
            if (component == null) return;
            var damage = attacker.GetCurrentDamage();
            component.CurrentHitPoint -= damage;
            _damageDisplay.DisplayDamage(damage, point);
            if (component.CurrentHitPoint <= 0)
            {
                Object.Destroy(target);
            }
        }
    }
}
