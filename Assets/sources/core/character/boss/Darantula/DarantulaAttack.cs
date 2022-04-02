using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    class DarantulaAttack : BossAttackData
    {
        [SerializeField]
        private BossParticleCollision _poison;
        private AbsorbComponent _absorber;

        protected override void Init()
        {
            CharacterSize = 10;
            _absorber = GetComponentInChildren<AbsorbComponent>();
            base.Init();
        }
        public void PoisoinAttack()
        {
            _poison.Attack();
        }
        public void EatingAttack()
        {
            _absorber.Attack();
        }
        public void StopEatingAttack()
        {
            _absorber.StopAttacking();
        }
        public override void StopAllAttacking()
        {
            _absorber.StopAttacking();
            base.StopAllAttacking();
        }
        public void StartEating()
        {
            _absorber.StartAbsorbing();
        }
        internal override void UpdateStatForBoss(int level)
        {
            var value = 0.8f + (level * 0.25f);
            _stat.MultipleDamage(value);
            _stat.DefenceMin += (level - 1) * 0.005f;
            _stat.DefenceMax += (level - 1) * 0.01f;
            Boss.SetMaximumHitPoint(Mathf.RoundToInt(_stat.HitPoint * value));
            _stat.AddCriticalResistance(level * 0.05f);
            _stat.AddStaggerResistance(level * 0.05f);
            _stat.AddFireResistance(level * 0.03f);
            _stat.AddIceResistance(level * 0.03f);
            _stat.AddSleepResistance(level * 0.03f);
        }
    }
}
