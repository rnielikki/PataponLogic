using UnityEngine;

namespace PataRoad.Core.Character.Animal
{
    class AttackCollision : MonoBehaviour
    {
        [SerializeField]
        AnimalBehaviour _attacker;
        [SerializeField]
        bool _useAdditionalStat;
        [SerializeField]
        Stat _additionalStat;
        private Stat _stat;
        private void Awake()
        {
            _stat = _attacker.Stat;
            if (_useAdditionalStat) _stat += _additionalStat;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Equipments.Logic.DamageCalculator.DealDamage(_attacker, _stat, collision.gameObject, collision.GetContact(0).point);
        }
    }
}
