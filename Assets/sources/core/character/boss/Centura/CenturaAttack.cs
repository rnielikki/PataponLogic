using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    class CenturaAttack : DarantulaAttack
    {
        internal override void UpdateStatForBoss(int level)
        {
            var value = 0.8f + (level * 0.4f);
            _stat.MultipleDamage(value);
            _stat.DefenceMin += (level - 1) * 0.01f;
            _stat.DefenceMax += (level - 1) * 0.02f;
            Boss.SetMaximumHitPoint(Mathf.RoundToInt(_stat.HitPoint * value));
            _stat.AddCriticalResistance(level * 0.08f);
            _stat.AddStaggerResistance(level * 0.1f);
            _stat.AddFireResistance(level * 0.1f);
            _stat.AddIceResistance(level * 0.08f);
            _stat.AddSleepResistance(level * 0.08f);
        }
    }
}