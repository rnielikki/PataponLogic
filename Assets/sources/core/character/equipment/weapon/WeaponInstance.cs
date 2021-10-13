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
        /// <summary>
        /// Yaripon throwing attack with physics.
        /// </summary>
        /// <param name="seconds">The delay as seconds before throwing.</param>
        private void Awake()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        }
        /// <summary>
        /// Set image sprite (how does it look).
        /// </summary>
        /// <param name="sprite"></param>
        public void SetSprite(Sprite sprite) =>
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;

        /// <summary>
        /// Start throwing this instance.
        /// </summary>
        public void Throw()
        {
            _rigidbody.AddForce(transform.up);
        }
        private void Update()
        {
            var velo = _rigidbody.velocity;
            transform.eulerAngles = Vector3.back * Mathf.Atan2(velo.x, velo.y) * Mathf.Rad2Deg;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy(gameObject);
        }
    }
}
