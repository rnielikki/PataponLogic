using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class BossAttackParticle : BossAttackComponent
    {
        private ParticleSystem _particleSystem;
        private ParticleSystem.Particle[] _particles;
        private LayerMask _layermask;
        private ParticleSystem.TriggerModule _trigger;
        private DistanceCalculator _distanceCalculator;

        private void Awake()
        {
            Init();
            _particleSystem = GetComponent<ParticleSystem>();
            var main = _particleSystem.main;
            _particles = new ParticleSystem.Particle[main.maxParticles];
            _trigger = _particleSystem.trigger;
            _particleSystem.collision.AddPlane(GameObject.FindGameObjectWithTag("Ground").transform);
        }
        private void Start()
        {
            _distanceCalculator = GetComponentInParent<ICharacter>().DistanceCalculator;
            _layermask = _distanceCalculator.LayerMask;
        }
        public void Attack()
        {
            for (int i = 0; i < _trigger.colliderCount; i++)
            {
                _trigger.RemoveCollider(0);
            }
            foreach (var hit in _distanceCalculator.GetAllAbsoluteTargetsOnFront())
            {
                _trigger.AddCollider(hit);
            }
            _particleSystem.Play();
        }

        private void OnParticleTrigger()
        {
            List<ParticleSystem.Particle> enteredParticles = new List<ParticleSystem.Particle>();
            var particlesCount = _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enteredParticles);

            for (int i = 0; i < particlesCount; i++)
            {
                ParticleSystem.Particle particle = enteredParticles[i];
                for (int j = 0; j < _trigger.colliderCount; j++)
                {
                    var collider = _trigger.GetCollider(j)?.GetComponent<Collider2D>();
                    //note: this checks "component is destroyed" so don't use ?.
                    if (collider != null && collider.gameObject != null)
                    {
                        if (collider.gameObject.CompareTag("Shield")) particle.remainingLifetime = 0;
                        else _boss.Attack(this, collider.gameObject, collider.ClosestPoint(particle.position),
                            _attackType, _elementalAttackType, true);
                    }
                }
            }
        }
    }
}
