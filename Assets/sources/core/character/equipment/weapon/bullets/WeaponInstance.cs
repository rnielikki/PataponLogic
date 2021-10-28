using System.Threading.Tasks;
using UnityEngine;

namespace Core.Character.Equipment.Weapon
{
    /// <summary>
    /// Instantiable weapon for calculating force, like Yaripon spear or Yumipon arrow.
    /// </summary>
    internal class WeaponInstance : MonoBehaviour
    {
        Rigidbody2D _rigidbody;
        Stat _stat;

        private void Awake()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        }
        public WeaponInstance SetHolderStat(Stat stat)
        {
            _stat = stat;
            return this;
        }
        /// <summary>
        /// Set image sprite (how does it look).
        /// </summary>
        /// <param name="sprite"></param>
        public WeaponInstance SetSprite(Sprite sprite)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            return this;
        }

        /// <summary>
        /// Start throwing this instance.
        /// </summary>
        /// <param name="forceMultiplier">With how much force will be thrown. 1 is normal force, 0 is no force.</param>
        public void Throw(float forceMultiplier)
        {
            _rigidbody.AddForce(transform.up * forceMultiplier);
        }
        private void Update()
        {
            var velo = _rigidbody.velocity;
            transform.eulerAngles = Vector3.back * Mathf.Atan2(velo.x, velo.y) * Mathf.Rad2Deg;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            DamageCalculator.DealDamage(_stat, collision.gameObject, collision.ClosestPoint(transform.position));
            Destroy(gameObject);
        }
    }
}
