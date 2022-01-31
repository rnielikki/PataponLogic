using UnityEngine;

namespace PataRoad.Core.Character.Animal
{
    class ParticleAttackCollision : MonoBehaviour
    {
        private ICharacter _attacker;
        private ParticleSystem _particleSystem;
        private Stat _stat;
        [SerializeField]
        private bool _useAdditionalStat;
        [SerializeField]
        private Stat _additionalStat;
        private System.Collections.Generic.List<ParticleCollisionEvent> _collisionEvents;
        [SerializeField]
        private Equipments.Weapons.AttackType _attackType;
        void Start()
        {
            _collisionEvents = new System.Collections.Generic.List<ParticleCollisionEvent>();
            _particleSystem = GetComponent<ParticleSystem>();
            _attacker = GetComponentInParent<ICharacter>();

            _stat = _attacker.Stat;
            if (_useAdditionalStat) _stat += _additionalStat;
        }
        private void OnParticleCollision(GameObject other)
        {
            int collisionCount = _particleSystem.GetCollisionEvents(other, _collisionEvents);
            for (int i = 0; i < collisionCount; i++)
            {
                Equipments.Logic.DamageCalculator.DealDamage(_attacker, _stat, other.gameObject, _collisionEvents[i].intersection, true);
            }
        }

    }
}
