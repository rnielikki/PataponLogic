using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    class BossParticleCollision : BossAttackComponent
    {
        private ParticleSystem _particleSystem;
        private System.Collections.Generic.List<ParticleCollisionEvent> _collisionEvents;
        private void Awake()
        {
            Init();
            _particleSystem = GetComponent<ParticleSystem>();
            _collisionEvents = new System.Collections.Generic.List<ParticleCollisionEvent>();
        }
        void Start()
        {
            _collisionEvents = new System.Collections.Generic.List<ParticleCollisionEvent>();
            _particleSystem = GetComponent<ParticleSystem>();
        }
        public void Attack()
        {
            _particleSystem.Play();
        }
        private void OnParticleCollision(GameObject other)
        {
            int collisionCount = _particleSystem.GetCollisionEvents(other, _collisionEvents);
            for (int i = 0; i < collisionCount; i++)
            {
                _boss.Attack(this, other, _collisionEvents[i].intersection, _attackType, _elementalAttackType, false);
            }
        }
    }
}
