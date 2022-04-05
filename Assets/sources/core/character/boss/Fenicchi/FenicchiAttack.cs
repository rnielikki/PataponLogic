namespace PataRoad.Core.Character.Bosses
{
    class FenicchiAttack : MochichichiAttack
    {
        internal override void UpdateStatForBoss(int level)
        {
            var value = 0.8f + (level * 0.4f);
            _stat.MultipleDamage(value);
            _stat.DefenceMin += (level - 1) * 0.02f;
            _stat.DefenceMax += (level - 1) * 0.02f;
            Boss.SetMaximumHitPoint(UnityEngine.Mathf.RoundToInt(_stat.HitPoint * value));
            _stat.AddCriticalResistance(level * 0.04f);
            _stat.AddStaggerResistance(level * 0.04f);
            _stat.AddKnockbackResistance(level * 0.04f);
            _stat.AddIceResistance(level * 0.02f);
            _stat.AddSleepResistance(level * 0.05f);
        }
    }
}