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
            UseCustomDataPosition = false;
            _absorber.StopAttacking();
            base.StopAllAttacking();
        }
        public void StartAbsorbing()
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
            _stat.CriticalResistance += level * 0.05f;
            _stat.StaggerResistance += level * 0.05f;
            _stat.FireResistance += level * 0.03f;
            _stat.IceResistance += level * 0.03f;
            _stat.SleepResistance += level * 0.03f;
        }
    }
}
