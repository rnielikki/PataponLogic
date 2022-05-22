using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    class ManborothAttack : BossAttackData
    {
        [SerializeField]
        ParticleSystem _iceInhaleParticles;
        [SerializeField]
        TriggerParticleEmitter _attackParticles;
        protected override void Init()
        {
            CharacterSize = 7;
            base.Init();
        }
        public void InhaleIce()
        {
            _iceInhaleParticles.Play();
        }
        public void ExhaleIce()
        {
            _attackParticles.Attack();
        }
        internal override void UpdateStatForBoss(int level)
        {
            var value = 0.8f + (level * 0.2f);
            _stat.MultipleDamage(value);
            _stat.DefenceMin += (level - 1) * 0.005f;
            _stat.DefenceMax += (level - 1) * 0.01f;
            Boss.SetMaximumHitPoint(Mathf.RoundToInt(_stat.HitPoint * value));
            _stat.AddCriticalResistance(level * 0.05f);
            _stat.AddStaggerResistance(level * 0.05f);
            _stat.AddKnockbackResistance(level * 0.05f);
            _stat.AddFireResistance(level * 0.03f);
            _stat.AddIceResistance(level * 0.03f);
            _stat.AddSleepResistance(level * 0.03f);
        }
    }
}
