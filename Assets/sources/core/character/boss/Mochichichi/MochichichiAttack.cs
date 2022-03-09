using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    class MochichichiAttack : BossAttackData
    {
        [SerializeField]
        private BossAttackParticle _fart;
        [SerializeField]
        private ParticleSystem _tornadoEffect;
        private Vector3 _targetPosition;

        protected override void Init()
        {
            CharacterSize = 4.3f;
            base.Init();
        }
        internal override void UpdateStatForBoss(int level)
        {
            var value = 0.8f + (level * 0.2f);
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
            StopIgnoringStatusEffect();
            _tornadoEffect.Play();
        }
        public void StartMoving()
        {
            _targetPosition = (_boss.DefaultWorldPosition + (20 * -_boss.MovingDirection.x)) * Vector3.right;
            UseCustomDataPosition = true;
        }
        public override void SetCustomPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _stat.MovementSpeed * 2);
            if (_targetPosition.x == transform.position.x)
            {
                UseCustomDataPosition = false;
                _boss.CharAnimator.Animate("Idle");
            }
        }
    }
}
