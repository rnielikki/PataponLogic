using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class Bow : Weapon
    {
        /// <summary>
        /// An arrow "transform" with animation, before shooting.
        /// </summary>
        private Transform _arrowTransform;
        /// <summary>
        /// copied arrow for throwing.
        /// </summary>
        private GameObject _copiedArrow;
        private static readonly Vector3 _throwAdditionalForce = Vector3.up;
        /// <summary>
        /// Minimal attack ditance, when is 100% headwind.
        /// </summary>
        public override float MinAttackDistance { get; } = 20;
        public override float WindAttackDistanceOffset { get; } = 10;

        private SpriteRenderer _bowRenderer;
        private SpriteRenderer _arrowRenderer;
        private void Awake()
        {
            LoadRenderersAndImage();
        }

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

            float minForce, maxForce;
            if (attackCommandType == AttackCommandType.Defend)
            {
                minForce = 300;
                maxForce = 480;
            }
            else
            {
                minForce = 840;
                maxForce = 1020;
            }

            arrowForThrowing.GetComponent<WeaponInstance>()
                .Initialize(this, 0.175f, transformOriginal: _arrowTransform)
                .Throw(minForce, maxForce, _throwAdditionalForce);
        }
        protected override void LoadRenderersAndImage()
        {
            _bowRenderer = transform.Find("Bow").GetComponent<SpriteRenderer>();
            _arrowRenderer = transform.Find("Arrow").GetComponent<SpriteRenderer>();
        }
        protected override void ReplaceImage(EquipmentData equipmentData)
        {
            _bowRenderer.sprite = equipmentData.Image;
            _arrowRenderer.sprite = (equipmentData as BowData).ArrowImage;
        }
    }
}
