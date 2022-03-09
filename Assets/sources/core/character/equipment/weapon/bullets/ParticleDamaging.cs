using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
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
        private IAttacker _holder;
        private ParticleSystem _particleSystem;
        private Stat _stat;
        private System.Collections.Generic.List<ParticleCollisionEvent> _collisionEvents;
        private float _direction = 1;
        public void Init(IAttacker holder)
        {
            _collisionEvents = new System.Collections.Generic.List<ParticleCollisionEvent>();
            _particleSystem = GetComponent<ParticleSystem>();
            _holder = holder;

            if (holder != null)
            {
                var collision = _particleSystem.collision;
                collision.collidesWith = CharacterTypeDataCollection.GetCharacterDataByType(_holder).AttackTargetLayerMask;
                if (holder is SmallCharacter character)
                {
                    _direction = character.MovingDirection.x;
                    //Set Scaling mode to hierarchy. Why? The START SHAPE matters.
                    transform.localScale = new Vector3(character.MovingDirection.x, 1, 1);
                }
            }
        }
        public void Emit(int count, float startSpeed)
        {
            var main = _particleSystem.main;
            main.startSpeed = startSpeed * _direction;
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
