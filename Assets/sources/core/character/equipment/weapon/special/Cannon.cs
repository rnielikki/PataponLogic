using UnityEngine;

namespace Core.Character.Equipment.Weapon
{
    class Cannon : WeaponObject
    {
        private GameObject _bullet;
        private Transform _startTransform;
        private void Awake()
        {
            Init();
            _bullet = GetWeaponInstance("Suko-Bullet");
            _startTransform = transform.Find("Bullet-Position");
        }

        public override void Attack(AttackCommandType attackCommandType)
        {
            var bullet = Instantiate(_bullet, transform.root.parent);
            bullet.transform.position = _startTransform.position;
            bullet.GetComponent<WeaponCannonBullet>().enabled = true;
        }
    }
}
