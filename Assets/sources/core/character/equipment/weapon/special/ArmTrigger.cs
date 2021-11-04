using UnityEngine;

namespace Core.Character.Equipment.Weapon
{
    /// <summary>
    /// Sends arm trigger attack event to <see cref="WeaponObject"/>.
    /// </summary>
    internal class ArmTrigger : MonoBehaviour
    {
        private Collider2D _collider;
        private ICharacter _holder;
        private void Start()
        {
            _collider = GetComponent<Collider2D>();
            _holder = GetComponentInParent<ICharacter>();
        }
        public void EnableCollider() => _collider.enabled = true;
        public void DisableCollider() => _collider.enabled = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Logic.DamageCalculator.DealDamage(
                _holder,
                collision.gameObject,
                collision.ClosestPoint(transform.position)
                );
        }
    }
}
