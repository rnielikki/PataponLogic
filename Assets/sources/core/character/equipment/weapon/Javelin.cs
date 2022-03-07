using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class Javelin : Weapon
    {
        private void Start()
        {
            Init();
        }
        /// <summary>
        /// Throws spear, from CURRENT spear position and rotation FROM ANIMATION.
        /// </summary>
        public override void Attack(AttackCommandType attackCommandType)
        {
            switch (attackCommandType)
            {
                case AttackCommandType.Attack:
                    ThrowWeaponInstance(625, 875);
                    break;
                case AttackCommandType.FeverAttack:
                    ThrowWeaponInstance(750, 900, -15);
                    ThrowWeaponInstance(900, 1000);
                    ThrowWeaponInstance(750, 900, 15);
                    break;
                case AttackCommandType.Defend:
                    ThrowWeaponInstance(500, 550);
                    break;
            }
            void ThrowWeaponInstance(float minForce, float maxForce, int angle = 0)
            {
                var javelinForThrowing = _objectPool.Get();
                javelinForThrowing.transform.SetParent(transform.root.parent);
                if (angle != 0) javelinForThrowing.transform.Rotate(Vector3.forward * angle);
                javelinForThrowing.GetComponent<WeaponInstance>()
                    .Initialize(this, _material)
                    .Throw(minForce, maxForce);
            }
        }
        public override float AdjustAttackDistanceByYPosition(float attackDistance, float yDistance) =>
            AdjustThrowingAttackDistanceByYPosition(attackDistance, yDistance);
        public override float GetAttackDistance()
        {
            var weatherOffset = (Map.Weather.WeatherInfo.Current.Wind?.Magnitude ?? 0);
            return base.GetAttackDistance() + weatherOffset;
        }
    }
}
