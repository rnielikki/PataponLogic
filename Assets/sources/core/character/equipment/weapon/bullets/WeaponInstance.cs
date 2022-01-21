using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    /// <summary>
    /// Instantiable weapon for calculating force, like Yaripon spear or Yumipon arrow.
    /// </summary>
    internal class WeaponInstance : MonoBehaviour
    {
        Rigidbody2D _rigidbody;
        ICharacter _holder;
        Stat _stat;

        private void Awake()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Initialize values from WeaponObject.
        /// </summary>
        /// <param name="original">The original weapon object, which is copied from.</param>
        /// <param name="mass">Mass of the object. This will affect to Tailwind.</param>
        /// <param name="transformOriginal">Transform of the object. If not set, default value is transform of <paramref name="original"/>.</param>
        /// <returns>Self.</returns>
        public WeaponInstance Initialize(Weapon original, Color color, float mass = -1, Transform transformOriginal = null)
        {
            if (transformOriginal == null) transformOriginal = original.transform;
            _rigidbody.mass = (mass < 0) ? original.Mass : mass;
            _holder = original.Holder;
            var renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = original.ThrowableWeaponSprite;
            renderer.color = color;

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
            _stat = _holder.Stat;
            var force = Random.Range(forceMultiplierMin, forceMultiplierMax);
            _rigidbody.AddForce(transform.up * force * _rigidbody.mass);
        }
        private void Update()
        {
            var velo = _rigidbody.velocity;
            transform.eulerAngles = Vector3.back * Mathf.Atan2(velo.x, velo.y) * Mathf.Rad2Deg;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Logic.DamageCalculator.DealDamage(_holder, _stat, collision.gameObject, collision.ClosestPoint(transform.position));
            if (collision.tag != "Grass") Destroy(gameObject);
        }
    }
}
