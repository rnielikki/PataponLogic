using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Character.Equipment.Weapon
{
    public class WeaponBullet : MonoBehaviour
    {
        Collider2D _collider;
        [SerializeField]
        [Tooltip("Defines when to destroy AFTER GROUNDED, as SECONDS. Zero will cause immediate destroy after grounding.")]
        private float _destroyTime;
        [SerializeField]
        [Tooltip("Rotate object to the velocity direction before being grounded.")]
        private bool _rotateOverTime;
        private bool _grounded;
        private Rigidbody2D _rigidbody;
        public ICharacter Holder { get; set; }
        /// <summary>
        /// Defines behaviour when the bullet is grounded. Don't define any destroy instruction, it's on <see cref="_destroyTime"/>
        /// </summary>
        /// <remarks>Collider2D represents the bullet's own collider - ground collider shouldn't affect anything.</remarks>
        public UnityAction<Collider2D> GroundAction { get; set; }
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_collider.attachedRigidbody != null)
            {
                if (other.tag == "Ground")
                {
                    _grounded = true;
                    GroundAction(_collider);
                    StartCoroutine(DestroyAfterTime());
                }
                else
                {
                    Logic.DamageCalculator.DealDamage(Holder, other.gameObject, other.ClosestPoint(transform.position));
                }
            }
            System.Collections.IEnumerator DestroyAfterTime()
            {
                yield return new WaitForSeconds(_destroyTime);
                Destroy(gameObject);
            }
        }
        private void Update()
        {
            if (!_rotateOverTime || _grounded) return;
            var velo = _rigidbody.velocity;
            transform.eulerAngles = Vector3.back * Mathf.Atan2(velo.x, velo.y) * Mathf.Rad2Deg;
        }
    }
}
