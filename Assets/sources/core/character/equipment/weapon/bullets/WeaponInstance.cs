using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    /// <summary>
    /// Instantiable weapon for calculating force, like Yaripon spear or Yumipon arrow.
    /// </summary>
    internal class WeaponInstance : MonoBehaviour
    {
        Rigidbody2D _rigidbody;
        BoxCollider2D _collider;
        IAttacker _holder;
        Stat _stat;
        private static GameObject _resource;

        private void Awake()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            _collider = gameObject.GetComponent<BoxCollider2D>();
        }
        /// <summary>
        /// Gets resource of the weapon instance. Note that it's NOT INSTANTIATED yet.
        /// </summary>
        /// <returns>the weapon instance resource.</returns>
        public static GameObject GetResource()
        {
            if (_resource == null) _resource = Resources.Load("Characters/Equipments/PrefabBase/WeaponInstance") as GameObject;
            return _resource;
        }
        /// <summary>
        /// Initialize values from WeaponObject.
        /// </summary>
        /// <param name="original">The original weapon object, which is copied from.</param>
        /// <param name="mass">Mass of the object. This will affect to Tailwind.</param>
        /// <param name="transformOriginal">Transform of the object. If not set, default value is transform of <paramref name="original"/>.</param>
        /// <returns>Self, as initialized.</returns>
        public WeaponInstance Initialize(Weapon original, Material material, float mass = -1, Transform transformOriginal = null)
        {
            if (transformOriginal == null) transformOriginal = original.transform;

            _rigidbody.mass = (mass < 0) ? original.Mass : mass;

            return Initialize(original.Holder, original.ThrowableWeaponSprite, material, original.gameObject.layer,
                (mass < 0) ? original.Mass : mass, transformOriginal);
        }
        /// <summary>
        /// Initialize values from WeaponObject without weapon but just by holder.
        /// </summary>
        /// <param name="holder">The holder of this weapon, attacker.</param>
        /// <param name="mass">Mass of the object. This will affect to Tailwind.</param>
        /// <param name="transformOriginal">Transform of the object. If not set, default value is transform of <paramref name="original"/>.</param>
        /// <returns>Self, as initialized.</returns>
        public WeaponInstance Initialize(IAttacker holder, Sprite sprite, Material material,
            int layer, float mass, Transform transformOriginal)
        {
            _rigidbody.mass = mass;
            _holder = holder;
            var renderer = GetComponent<SpriteRenderer>();

            renderer.sprite = sprite;
            Weapon.SetColliderBoundingBox(_collider, sprite);
            renderer.material = material;

            transform.SetPositionAndRotation(transformOriginal.position, transformOriginal.rotation);

            gameObject.layer = layer;
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
            _rigidbody.AddForce(_rigidbody.mass * force * transform.up);
        }
        private void Update()
        {
            var velo = _rigidbody.velocity;
            transform.eulerAngles = Mathf.Atan2(velo.x, velo.y) * Mathf.Rad2Deg * Vector3.back;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Logic.DamageCalculator.DealDamage(_holder, _stat, collision.gameObject, collision.ClosestPoint(transform.position));
            if (!collision.CompareTag("Grass"))
            {
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
    }
}
