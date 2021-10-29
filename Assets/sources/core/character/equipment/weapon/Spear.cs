using UnityEngine;

namespace Core.Character.Equipment.Weapon
{
    class Spear : WeaponObject
    {
        /// <summary>
        /// copied spear for throwing.
        /// </summary>
        private GameObject _copiedSpear;
        private Sprite _sprite;
        private void Awake()
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

            spearForThrowing.GetComponent<WeaponInstance>()
                .Initialize(this)
                .Throw((attackCommandType == AttackCommandType.Defend) ? 0.5f : 0.75f);
        }
    }
}
