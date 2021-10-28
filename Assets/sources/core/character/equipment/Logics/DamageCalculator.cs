namespace Core.Character.Equipment
{
    static class DamageCalculator
    {
        public static void DealDamage(Stat attackerStat, UnityEngine.GameObject target, UnityEngine.Vector2 point)
        {
            var component = target.GetComponent<IAttackable>();
            if (component == null) return;
            component.CurrentHitPoint -= attackerStat.DamageMax;
            if (component.CurrentHitPoint < 0)
            {
                UnityEngine.Object.Destroy(target);
            }
        }
    }
}
