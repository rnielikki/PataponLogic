using UnityEngine;

namespace Core.Character.Equipment.Weapon
{
    class Bow : WeaponObject
    {
        /// <summary>
        /// An arrow "transform" with animation, before shooting.
        /// </summary>
        private Transform _arrowTransform;
        /// <summary>
        /// copied arrow for throwing.
        /// </summary>
        private GameObject _copiedArrow;
        private void Start()
        {
            _arrowTransform = transform.Find("Arrow");
            Init();
            _copiedArrow = GetWeaponInstance();
        }
        protected override Sprite GetThrowableWeaponSprite() => _arrowTransform.GetComponent<SpriteRenderer>().sprite;
        /// <summary>
        /// Throws spear, from CURRENT spear position and rotation FROM ANIMATION.
        /// </summary>
        public override void Attack(AttackCommandType attackCommandType)
        {
            var arrowForThrowing = Instantiate(_copiedArrow, transform.root.parent);

            arrowForThrowing.GetComponent<WeaponInstance>()
                .Initialize(this, _arrowTransform)
                .Throw((attackCommandType == AttackCommandType.Defend) ? 1 : 2);
        }
    }
}
