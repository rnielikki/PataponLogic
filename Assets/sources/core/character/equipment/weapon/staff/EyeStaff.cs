using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    public class EyeStaff : AttackCollisionStaff
    {
        private ParticleSystem _particles;

        public override void Initialize(SmallCharacter holder)
        {
            _particles = GetComponent<ParticleSystem>();
            base.Initialize(holder);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (AttackOnTrigger(collision))
            {
                _particles.Play();
            }
        }
    }
}
