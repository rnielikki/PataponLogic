using UnityEngine;

namespace PataRoad.Core.Character.Animal
{
    class AttackCollision : MonoBehaviour
    {
        [SerializeField]
        AnimalBehaviour _attacker;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Equipments.Logic.DamageCalculator.DealDamage(_attacker, _attacker.Stat, collision.gameObject, collision.GetContact(0).point);
        }
    }
}
