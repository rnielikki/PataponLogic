using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    class MochichichiAttack : BossAttackData
    {
        [SerializeField]
        private TriggerParticleEmitter _fart;
        [SerializeField]
        private ParticleSystem _tornadoEffect;
        private Vector3 _targetPosition;
        private EnemyBoss _enemyBoss;
        private bool _moving;

        protected override void Init()
        {
            CharacterSize = 4.3f;
            base.Init();
            _enemyBoss = Boss as EnemyBoss;

        }

        internal override void UpdateStatForBoss(int level)
        {
            var value = 0.8f + (level * 0.2f);
            _stat.MultipleDamage(value);
            _stat.DefenceMin += (level - 1) * 0.01f;
            _stat.DefenceMax += (level - 1) * 0.015f;
            Boss.SetMaximumHitPoint(Mathf.RoundToInt(_stat.HitPoint * value));
            _stat.AddCriticalResistance(level * 0.04f);
            _stat.AddStaggerResistance(level * 0.04f);
            _stat.AddKnockbackResistance(level * 0.04f);
            _stat.AddFireResistance(level * 0.02f);
            _stat.AddIceResistance(level * 0.02f);
            _stat.AddSleepResistance(level * 0.02f);
        }
        public void FartAttack()
        {
            _fart.Attack();
            _targetPosition = transform.position + 40 * Vector3.right;
        }
        public void TornadoAttack()
        {
            StopIgnoringStatusEffect();
            _tornadoEffect.Play();
        }
        public void StartMoving()
        {
            if (_enemyBoss == null) return;
            _targetPosition = transform.position + 40 * Vector3.right;
            _moving = true;
        }
        public override void SetCustomPosition()
        {
            if (!_moving) return;
            transform.position += Time.deltaTime * _stat.MovementSpeed * 2 * Vector3.right;
            if (_targetPosition.x <= transform.position.x)
            {
                Boss.CharAnimator.Animate("Idle");
                UseCustomDataPosition = false;
                _moving = false;
            }
        }
    }
}
