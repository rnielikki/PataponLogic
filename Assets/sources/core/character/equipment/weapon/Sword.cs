using UnityEngine;

namespace Core.Character.Equipment.Weapon
{
    class Sword : WeaponObject
    {
        private void Awake()
        {
            Init();
        }
        /// <summary>
        /// Throws spear, from CURRENT spear position and rotation FROM ANIMATION.
        /// </summary>
        public override void Attack(AttackType attackType)
        {
        }
    }
}
