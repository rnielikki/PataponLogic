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
        public UnityAction<Collider2D, Vector2> GroundAction { get; private set; }
        public UnityAction<Collider2D> CollidingAction { get; private set; }
        private Vector2 _direction;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }
        internal void SetHolder(SmallCharacter holder, Stat stat,
            UnityAction<Collider2D> collidingAction, UnityAction<Collider2D, Vector2> groundAction)
        {
            Holder = holder;
            _direction = holder.MovingDirection;

            Stat = stat;
            CollidingAction = collidingAction;
            GroundAction = groundAction;
        }
        internal void Throw(Vector2 force)
        {
            _rigidbody.WakeUp();
            _rigidbody.gravityScale = 1;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(force);
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_collider.attachedRigidbody != null)
            {
                if (other.CompareTag("Ground"))
                {
                    OnGround();
                }
                else
                {
                    if (CollidingAction != null) CollidingAction(other);
                    Logic.DamageCalculator.DealDamage(Holder, Stat, other.gameObject, other.ClosestPoint(transform.position));
                }
            }
        }
        private void OnGround()
        {
            _grounded = true;
            GroundAction(_collider, _direction);
            StartCoroutine(DestroyAfterTime());
            System.Collections.IEnumerator DestroyAfterTime()
            {
                yield return new WaitForSeconds(_destroyTime);
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.gravityScale = 1;
                _rigidbody.constraints = RigidbodyConstraints2D.None;
                _rigidbody.Sleep();

                var rel = gameObject.GetComponent<ReleaseToPool>();
                if (rel != null)
                {
                    rel.ReleaseThisObject();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        private void Update()
        {
            if (!_rotateOverTime || _grounded) return;
            else if (transform.position.y < -1)
            {
                OnGround();
                return;
            }
            var velo = _rigidbody.velocity;
            transform.eulerAngles = Mathf.Atan2(velo.x, velo.y) * Mathf.Rad2Deg * Vector3.back;
        }
    }
}
