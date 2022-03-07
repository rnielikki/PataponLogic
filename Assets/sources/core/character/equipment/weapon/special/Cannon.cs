using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class Cannon : Weapon
    {
        private Transform _startTransform;

        private void Start()
        {
            resourcePath = "Characters/Equipments/PrefabBase/Suko-Bullet";
            poolInitialSize = 3;
            poolMaxSize = 10;
            Init();
            _startTransform = transform.Find("Bullet-Position");
        }

        public override void Attack(AttackCommandType attackCommandType)
        {
            var bullet = _objectPool.Get();
            bullet.transform.position = _startTransform.position;
            bullet.layer = gameObject.layer;
            var bulletScript = bullet.GetComponent<WeaponCannonBullet>();
            bulletScript.Init(Holder);
        }
        public override float GetAttackDistance() => 15;
    }
}
