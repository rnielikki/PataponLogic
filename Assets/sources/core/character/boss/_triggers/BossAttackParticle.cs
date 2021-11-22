using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class BossAttackParticle : BossAttackComponent
    {
        private ParticleSystem _particleSystem;
        private ParticleSystem.Particle[] _particles;
        private LayerMask _layermask;
        private HashSet<Collider2D> _collided;
        private ParticleSystem.TriggerModule _trigger;

        private void Awake()
        {
            Init();
            _particleSystem = GetComponent<ParticleSystem>();
            var main = _particleSystem.main;
            _particles = new ParticleSystem.Particle[main.maxParticles];
            _collided = new HashSet<Collider2D>();
            _trigger = _particleSystem.trigger;
        }
        private void Start()
        {
            _layermask = GetComponentInParent<ICharacter>().DistanceCalculator.LayerMask;
        }
        /*
        private void Update()
        {
            if (_particleSystem.particleCount != 0)
            {
                var allParticles = _particleSystem.GetParticles(_particles);
                for (int i = 0; i < allParticles; i++)
                {
                    ParticleSystem.Particle.compa
                    var p0 = _particles[i];
                    var pos = p0.position;
                    var size = p0.GetCurrentSize(_particleSystem);
                    foreach (var hit in Physics2D.OverlapCircleAll(pos, size, _layermask))
                    {
                        if (hit.gameObject == null) continue;
                        if (!_particleMap.ContainsKey(hit.gameObject))
                        {
                            _particleMap.Add(hit.gameObject, new List<ParticleSystem.Particle>());
                        }
                        if (_particleMap[hit.gameObject].Contains(p0)) continue;

                        _particleMap[hit.gameObject].Add(p0);
                        ParticleSystem.Particle p = _particles[i];
                        _boss.Attack(this, hit.gameObject, hit.ClosestPoint(transform.position));
                    }
                }
            }
        }
        */
        private void OnParticleTrigger()
        {
            var allParticles = _particleSystem.GetParticles(_particles);
            for (int i = 0; i < allParticles; i++)
            {
                var p0 = _particles[i];
                var pos = p0.position;
                var size = p0.GetCurrentSize(_particleSystem);
                foreach (var hit in Physics2D.OverlapCircleAll(pos, size, _layermask))
                {
                    if (hit.gameObject == null) continue;
                    if (!_collided.Contains(hit))
                    {
                        _collided.Add(hit);
                        _trigger.AddCollider(hit);
                    }
                }
            }

            List<ParticleSystem.Particle> enteredParticles = new List<ParticleSystem.Particle>();
            _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enteredParticles);

            foreach (ParticleSystem.Particle particle in enteredParticles)
            {
                for (int i = 0; i < _trigger.colliderCount; i++)
                {
                    var collider = _trigger.GetCollider(i)?.GetComponent<Collider2D>();
                    if (collider?.gameObject != null) _boss.Attack(this, collider.gameObject, collider.ClosestPoint(particle.position));
                }
            }
        }
    }
}
