using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class ShooshookleAttack : BossAttackData
    {
        [SerializeField]
        private BossAttackParticle _spore;
        protected override void Init()
        {
            CharacterSize = 5;
            base.Init();
        }
        public void SporeAttack()
        {
            _spore.Attack();
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
            _stat.FireResistance += level * 0.05f;
            _stat.IceResistance += level * 0.05f;

        }
    }
}