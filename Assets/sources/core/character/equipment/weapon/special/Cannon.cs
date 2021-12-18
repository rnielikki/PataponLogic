using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class Cannon : Weapon
    {
        private GameObject _bullet;
        private Transform _startTransform;

        private void Start()
        {
            Init();
            _bullet = GetWeaponInstance("Suko-Bullet");
            _startTransform = transform.Find("Bullet-Position");
        }

        public override void Attack(AttackCommandType attackCommandType)
        {
            var bullet = Instantiate(_bullet, transform.root.parent);
            bullet.transform.position = _startTransform.position;
            bullet.layer = gameObject.layer;
            var bulletScript = bullet.GetComponent<WeaponCannonBullet>();
            bulletScript.Holder = Holder;
            bulletScript.enabled = true;
        }
        public override float GetAttackDistance()
        {
            var weatherOffset = (Map.Weather.WeatherInfo.Current.Wind?.Magnitude ?? 0);
            return base.GetAttackDistance() + weatherOffset;
        }
    }
}
