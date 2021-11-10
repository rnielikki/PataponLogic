using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    /// <summary>
    /// Represents cannon bullet for sukopon weapon. Unlike <see cref="WeaponBullet"/>, it destroys self after certain distance from start.
    /// </summary>
    /// <note>Don't use other direction than 'to the right' direction (the Patapon direction).</note>
    public class WeaponCannonBullet : MonoBehaviour
    {
        [SerializeField]
        private float _maxDistance;
        private float _bulletSpeed;
        private float _destroyDistance;
        public ICharacter Holder { get; set; }
        private Stat _stat;
        // Start is called before the first frame update
        void Start()
        {
            _bulletSpeed = _maxDistance / Rhythm.RhythmEnvironment.TurnSeconds;
            _destroyDistance = transform.position.x + _maxDistance;
            _stat = Holder.Stat;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(_bulletSpeed * Time.deltaTime, 0, 0);
            if (_destroyDistance <= transform.position.x)
            {
                Destroy(gameObject);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Logic.DamageCalculator.DealDamage(Holder, _stat, collision.gameObject, collision.ClosestPoint(transform.position));
        }
    }
}
