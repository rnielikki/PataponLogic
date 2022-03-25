using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class GaruruBall : BossAttackTrigger
    {
        ParticleSystem _particle;
        // Use this for initialization
        void Start()
        {
            _particle = GetComponent<ParticleSystem>();
        }

        public void Show()
        {
            if (!_particle.isPlaying) _particle.Play();
        }
        public override void StopAttacking()
        {
            base.StopAttacking();
            _particle.Stop();
        }
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (_enabled)
            {
                base.OnTriggerEnter2D(collision);
                StopAttacking();
            }
        }
    }
}