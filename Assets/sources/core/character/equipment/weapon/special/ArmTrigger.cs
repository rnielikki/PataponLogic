using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    /// <summary>
    /// Sends arm trigger attack event to <see cref="Weapon"/>.
    /// </summary>
    internal class ArmTrigger : MonoBehaviour
    {
        private BoxCollider2D _collider;
        private SpriteRenderer _renderer;
        private ICharacter _holder;
        private Stat _stat;
        private void Start()
        {
            Init();
            _holder = GetComponentInParent<ICharacter>();
        }
        private void Init()
        {
            if (_renderer != null) return;
            _renderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider2D>();
        }
        public void EnableAttacking(Stat stat)
        {
            _stat = stat;
            _collider.enabled = true;
        }
        internal void SetColliderSize(Sprite sprite)
        {
            if (_collider == null) Init();
            Weapon.SetColliderBoundingBox(_collider, sprite, _renderer.flipX, _renderer.flipY);
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
