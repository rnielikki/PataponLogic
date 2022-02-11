using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Character.Equipments.Weapons
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
        public SmallCharacter Holder { get; set; }
        public Stat Stat { get; set; }
        /// <summary>
        /// Defines behaviour when the bullet is grounded. Don't define any destroy instruction, it's on <see cref="_destroyTime"/>
        /// </summary>
        /// <remarks>Collider2D represents the bullet's own collider - ground collider shouldn't affect anything.</remarks>
        public UnityAction<Collider2D, Vector2> GroundAction { get; set; }
        public UnityAction<Collider2D> CollidingAction { get; internal set; }
        private Vector2 _direction;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }
        private void SetHolder(SmallCharacter holder)
        {
            Holder = holder;
            _direction = holder.MovingDirection;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_collider.attachedRigidbody != null)
            {
                if (other.tag == "Ground")
                {
                    _grounded = true;
                    GroundAction(_collider, _direction);
                    StartCoroutine(DestroyAfterTime());
                }
                else
                {
                    if (CollidingAction != null) CollidingAction(other);
                    Logic.DamageCalculator.DealDamage(Holder, Stat, other.gameObject, other.ClosestPoint(transform.position));
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
