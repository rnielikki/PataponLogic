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
        private Sprite _arrowSprite;
        private void Awake()
        {
            Init();
            _arrowTransform = transform.Find("Arrow");
            _copiedArrow = GetWeaponInstance();
            _arrowSprite = _arrowTransform.GetComponent<SpriteRenderer>().sprite;
        }
        /// <summary>
        /// Throws spear, from CURRENT spear position and rotation FROM ANIMATION.
        /// </summary>
        public override void Attack(AttackType attackType)
        {
            var arrowForThrowing = Instantiate(_copiedArrow, transform.root.parent);
            arrowForThrowing.GetComponent<SpriteRenderer>().sprite = _arrowSprite;
            arrowForThrowing.transform.position = _arrowTransform.position;
            arrowForThrowing.transform.rotation = _arrowTransform.rotation;
            arrowForThrowing.GetComponent<WeaponInstance>().Throw((attackType == AttackType.Defend) ? 1 : 1.5f);
        }
    }
}
