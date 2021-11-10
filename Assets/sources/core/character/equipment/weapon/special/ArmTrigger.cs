using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    /// <summary>
    /// Sends arm trigger attack event to <see cref="Weapon"/>.
    /// </summary>
    internal class ArmTrigger : MonoBehaviour
    {
        private Collider2D _collider;
        private ICharacter _holder;
        private Stat _stat;
        private void Start()
        {
            _collider = GetComponent<Collider2D>();
            _holder = GetComponentInParent<ICharacter>();
        }
        public void EnableAttacking(Stat stat)
        {
            _stat = stat;
            _collider.enabled = true;
        }
        public void DisableAttacking() => _collider.enabled = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Logic.DamageCalculator.DealDamage(
                _holder,
                _stat,
                collision.gameObject,
                collision.ClosestPoint(transform.position)
                );
        }
    }
}
