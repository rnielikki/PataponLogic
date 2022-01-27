using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    class MochichichiAttack : BossAttackData
    {
        [SerializeField]
        private BossAttackParticle _fart;
        [SerializeField]
        private ParticleSystem _tornadoEffect;
        [SerializeField]
        private BossAttackCollision _tornado;
        [SerializeField]
        private BossAttackCollision _slam;
        [SerializeField]
        private BossAttackTrigger _peck;

        internal override void UpdateStatForBoss(int level)
        {
            var value = 0.8f + level * 0.2f;
            _stat.MultipleDamage(value);
            _stat.DefenceMin += (level - 1) * 0.01f;
            _stat.DefenceMax += (level - 1) * 0.015f;
            _boss.SetMaximumHitPoint(Mathf.RoundToInt(_stat.HitPoint * value));
            _stat.CriticalResistance += level * 0.04f;
            _stat.StaggerResistance += level * 0.04f;
            _stat.KnockbackResistance += level * 0.04f;
            _stat.FireResistance += level * 0.02f;
            _stat.IceResistance += level * 0.02f;
            _stat.SleepResistance += level * 0.02f;
        }
        public void FartAttack()
        {
            _fart.Attack();
        }
        public void TornadoAttack()
        {
            _tornadoEffect.Play();
        }
        public void SlamAttack()
        {
            _slam.Attack();
        }
        public void StopSlamAttack()
        {
            _slam.StopAttacking();
        }

        public override void StopAllAttacking()
        {
            //hmm...
            StopSlamAttack();
        }
    }
}
