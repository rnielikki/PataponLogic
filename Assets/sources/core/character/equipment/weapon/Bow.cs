﻿using PataRoad.Core.Items;
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

        private SpriteRenderer _bowRenderer;
        private SpriteRenderer _arrowRenderer;
        protected override float _throwMass => 0.175f;
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
                minForce = 1200;
                maxForce = 1400;
            }
            else
            {
                minForce = 1000;
                maxForce = 1200;
            }

            arrowForThrowing.GetComponent<WeaponInstance>()
                .Initialize(this, _color, _throwMass, transformOriginal: _arrowTransform)
                .Throw(minForce, maxForce, _throwAdditionalForce);
        }
        protected override void LoadRenderersAndImage()
        {
            _bowRenderer = transform.Find("Bow").GetComponent<SpriteRenderer>();
            _arrowRenderer = transform.Find("Arrow").GetComponent<SpriteRenderer>();
            _spriteRenderers = new SpriteRenderer[] { _bowRenderer, _arrowRenderer };
        }
        protected override void ReplaceImage(EquipmentData equipmentData)
        {
            _bowRenderer.sprite = equipmentData.Image;
            _arrowRenderer.sprite = (equipmentData as BowData).ArrowImage;
        }
        internal override void SetLastAttackCommandType(AttackCommandType attackCommandType)
        {
            base.SetLastAttackCommandType(attackCommandType);
            //CHANGE ANGLE IF CHANGE ANIMATION.
            switch (attackCommandType)
            {
                case AttackCommandType.Attack:
                case AttackCommandType.FeverAttack:
                    SetInitialVelocity(1400, 40.461f, _throwAdditionalForce);
                    break;
                case AttackCommandType.Defend:
                    SetInitialVelocity(1100, 16.785f, _throwAdditionalForce);
                    break;
            }
        }
        public override float GetAttackDistance() => GetThrowingAttackDistance();
    }
}
