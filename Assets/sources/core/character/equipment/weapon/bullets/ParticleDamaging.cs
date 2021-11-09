using UnityEngine;

namespace PataRoad.Core.Character.Equipment.Weapon
{
    /// <summary>
    /// Attach this to <see cref="ParticleSystem"/> to attack, like Megapon non-fever attack.
    /// </summary>
    /// <note> 
    /// * IMPORTANT: Particle System Component -> Collision -> *** Send Collision Messages *** on! Otherwise it won't work!
    /// * Make sure that the holder is somewhere in parent.
    /// </note>
    public class ParticleDamaging : MonoBehaviour
    {
        private ICharacter _holder;
        private ParticleSystem _particleSystem;
        private Stat _stat;
        private System.Collections.Generic.List<ParticleCollisionEvent> _collisionEvents;
        // Start is called before the first frame update
        void Awake()
        {
            _collisionEvents = new System.Collections.Generic.List<ParticleCollisionEvent>();
            _particleSystem = GetComponent<ParticleSystem>();
            _holder = GetComponentInParent<ICharacter>();
        }
        public void Emit(int count, int startSpeed)
        {
            var main = _particleSystem.main;
            main.startSpeed = startSpeed;
            _stat = _holder.Stat;
            _particleSystem.Emit(count);
        }

        private void OnParticleCollision(GameObject other)
        {
            int collisionCount = _particleSystem.GetCollisionEvents(other, _collisionEvents);
            for (int i = 0; i < collisionCount; i++)
            {
                Logic.DamageCalculator.DealDamage(_holder, _stat, other.gameObject, _collisionEvents[i].intersection);
            }
        }
    }
}