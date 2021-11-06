using UnityEngine;

namespace PataRoad.Core.Character.Equipment.Weapon
{
    /// <summary>
    /// Instantiable weapon for calculating force, like Yaripon spear or Yumipon arrow.
    /// </summary>
    internal class WeaponInstance : MonoBehaviour
    {
        Rigidbody2D _rigidbody;
        ICharacter _holder;

        private void Awake()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Initialize values from WeaponObject.
        /// </summary>
        /// <param name="original">The original weapon object, which is copied from.</param>
        /// <param name="transform">Transform of the object.</param>
        /// <returns>Self.</returns>
        public WeaponInstance Initialize(WeaponObject original, Transform transformOriginal = null)
        {
            if (transformOriginal == null) transformOriginal = original.transform;
            _holder = original.Holder;
            GetComponent<SpriteRenderer>().sprite = original.ThrowableWeaponSprite;

            transform.position = transformOriginal.position;
            transform.rotation = transformOriginal.rotation;

            gameObject.layer = original.gameObject.layer;

            return this;
        }

        /// <summary>
        /// Start throwing this instance.
        /// </summary>
        /// <param name="forceMultiplierMin">Minimum value of force that will be thrown with. 1 is normal force, 0 is no force.</param>
        /// <param name="forceMultiplierMax">Maximum value of force that will be thrown with. 1 is normal force, 0 is no force.</param>
        public void Throw(float forceMultiplierMin, float forceMultiplierMax)
        {
            _rigidbody.AddForce(transform.up * Random.Range(forceMultiplierMin, forceMultiplierMax));
        }
        private void Update()
        {
            var velo = _rigidbody.velocity;
            transform.eulerAngles = Vector3.back * Mathf.Atan2(velo.x, velo.y) * Mathf.Rad2Deg;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Logic.DamageCalculator.DealDamage(_holder, collision.gameObject, collision.ClosestPoint(transform.position));
            Destroy(gameObject);
        }
    }
}
