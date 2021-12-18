using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class Spear : Weapon
    {
        /// <summary>
        /// copied spear for throwing.
        /// </summary>
        private GameObject _copiedSpear;
        private void Start()
        {
            Init();
            _copiedSpear = GetWeaponInstance();
        }
        /// <summary>
        /// Throws spear, from CURRENT spear position and rotation FROM ANIMATION.
        /// </summary>
        public override void Attack(AttackCommandType attackCommandType)
        {
            var spearForThrowing = Instantiate(_copiedSpear, transform.root.parent);
            float minForce, maxForce;
            if (attackCommandType == AttackCommandType.Defend)
            {
                minForce = 300;
                maxForce = 600;
            }
            else
            {
                minForce = 800;
                maxForce = 950;
            }

            spearForThrowing.GetComponent<WeaponInstance>()
                .Initialize(this)
                .Throw(minForce, maxForce);
        }
        public override float GetAttackDistance()
        {
            var weatherOffset = (Map.Weather.WeatherInfo.Current.Wind?.Magnitude ?? 0);
            return base.GetAttackDistance() + weatherOffset;
        }
    }
}
