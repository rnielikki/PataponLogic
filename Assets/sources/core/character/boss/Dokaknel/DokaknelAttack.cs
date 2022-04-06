namespace PataRoad.Core.Character.Bosses
{
    class DokaknelAttack : ZaknelAttack
    {
        internal override void UpdateStatForBoss(int level)
        {
            var value = 0.8f + (level * 0.4f);
            _stat.MultipleDamage(value);
            _stat.DefenceMin += (level - 1) * 0.01f;
            _stat.DefenceMax += (level - 1) * 0.01f;
            Boss.SetMaximumHitPoint(UnityEngine.Mathf.RoundToInt(_stat.HitPoint * value));

            Stat.AddCriticalResistance(level * 0.08f);
            Stat.AddStaggerResistance(level * 0.1f);

            Stat.AddFireResistance(level * 0.05f);
            Stat.AddIceResistance(level * 0.05f);
            Stat.AddSleepResistance(level * 0.08f);
        }
    }
}