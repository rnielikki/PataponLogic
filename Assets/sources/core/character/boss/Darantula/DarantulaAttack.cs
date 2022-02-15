using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    class DarantulaAttack : BossAttackData
    {
        [SerializeField]
        private BossParticleCollision Poison;
        [SerializeField]
        private DarantulaAbsorbComponent Absorber;

        public void PoisoinAttack()
        {
            Poison.Attack();
        }
        public void EatingAttack()
        {
            Absorber.Attack();
        }
        public void StopEatingAttack()
        {
            Absorber.StopAttacking();
        }
        public override void StopAllAttacking()
        {
            Absorber.StopAttacking();
            base.StopAllAttacking();
        }
        internal override void UpdateStatForBoss(int level)
        {
            var value = 0.8f + level * 0.25f;
            _stat.MultipleDamage(value);
            _stat.DefenceMin += (level - 1) * 0.005f;
            _stat.DefenceMax += (level - 1) * 0.01f;
            _boss.SetMaximumHitPoint(Mathf.RoundToInt(_stat.HitPoint * value));
            _stat.CriticalResistance += level * 0.05f;
            _stat.StaggerResistance += level * 0.05f;
            _stat.FireResistance += level * 0.03f;
            _stat.IceResistance += level * 0.03f;
            _stat.SleepResistance += level * 0.03f;
        }
    }
}
