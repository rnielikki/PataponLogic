using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class DogaeenAttack : BossAttackData
    {
        [SerializeField]
        DogaeenRepel _repel;
        protected override void Init()
        {
            CharacterSize = 15;
            base.Init();
        }
        public void RepelAttack()
        {
            _repel.Repel();
        }

        internal override void UpdateStatForBoss(int level)
        {
            var value = 0.8f + (level * 0.2f);
            _stat.MultipleDamage(value);
            _stat.DefenceMin += (level - 1) * 0.005f;
            _stat.DefenceMax += (level - 1) * 0.01f;
            Boss.SetMaximumHitPoint(Mathf.RoundToInt(_stat.HitPoint * value));
            _stat.CriticalResistance += level * 0.05f;
            _stat.StaggerResistance += level * 0.05f;
            _stat.KnockbackResistance += level * 0.05f;
            _stat.FireResistance += level * 0.03f;
            _stat.IceResistance += level * 0.03f;
            _stat.SleepResistance += level * 0.03f;
        }
    }
}
