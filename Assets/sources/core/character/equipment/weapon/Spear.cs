using UnityEngine;

namespace PataRoad.Core.Character.Equipment.Weapon
{
    class Spear : WeaponObject
    {
        /// <summary>
        /// copied spear for throwing.
        /// </summary>
        private GameObject _copiedSpear;
        private Sprite _sprite;
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
                minForce = 50;
                maxForce = 75;
            }
            else
            {
                minForce = 95;
                maxForce = 102;
            }

            spearForThrowing.GetComponent<WeaponInstance>()
                .Initialize(this)
                .Throw(minForce, maxForce);
        }
    }
}
