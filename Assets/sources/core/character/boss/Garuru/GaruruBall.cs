using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class GaruruBall : BossAttackTrigger
    {
        ParticleSystem _particle;
        // Use this for initialization
        void Awake()
        {
            Init();
            _particle = GetComponent<ParticleSystem>();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            if (!_particle.isPlaying) _particle.Play();
        }
        public override void StopAttacking()
        {
            base.StopAttacking();
            gameObject.SetActive(false);
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