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

        private SpriteRenderer _bowRenderer;
        private SpriteRenderer _arrowRenderer;
        protected override float _throwMass => 0.175f;
        public override bool IsTargetingCenter => true;
        private void Awake()
        {
            LoadRenderersAndImage();
        }

        private void Start()
        {
            _arrowTransform = transform.Find("Arrow");
            Init();
        }
        protected override Sprite GetThrowableWeaponSprite() => _arrowTransform.GetComponent<SpriteRenderer>().sprite;
        /// <summary>
        /// Throws spear, from CURRENT spear position and rotation FROM ANIMATION.
        /// </summary>
        public override void Attack(AttackCommandType attackCommandType)
        {
            var arrowForThrowing = _objectPool.Get();
            arrowForThrowing.transform.SetParent(transform.root.parent);
            float minThrowDistance = 1600;
            float maxThrowDistance = 1850;
            if (attackCommandType == AttackCommandType.Defend)
            {
                minThrowDistance = 1400;
                maxThrowDistance = 1700;
            }
            arrowForThrowing.GetComponent<WeaponInstance>()
                .Initialize(this, _material, _throwMass, transformOriginal: _arrowTransform)
                .Throw(minThrowDistance, maxThrowDistance);
        }
        protected override void LoadRenderersAndImage()
        {
            if (_bowRenderer != null) return;
            _bowRenderer = transform.Find("Bow").GetComponent<SpriteRenderer>();
            _arrowRenderer = transform.Find("Arrow").GetComponent<SpriteRenderer>();
            _spriteRenderers = new SpriteRenderer[] { _bowRenderer, _arrowRenderer };
            _material = _spriteRenderers[0].material;
        }
        protected override void ReplaceImage(EquipmentData equipmentData)
        {
            if (_bowRenderer == null) LoadRenderersAndImage();
            _bowRenderer.sprite = equipmentData.Image;
            _arrowRenderer.sprite = (equipmentData as BowData).ArrowImage;
        }
        public override float AdjustAttackDistanceByYPosition(float attackDistance, float yDistance) =>
            AdjustThrowingAttackDistanceByYPosition(attackDistance, yDistance);
        internal override void SetLastAttackCommandType(AttackCommandType attackCommandType)
        {
            base.SetLastAttackCommandType(attackCommandType);
            //CHANGE ANGLE IF CHANGE ANIMATION.
            switch (attackCommandType)
            {
                case AttackCommandType.Attack:
                case AttackCommandType.FeverAttack:
                    SetInitialVelocity(1550, 60f);
                    break;
                case AttackCommandType.Defend:
                    SetInitialVelocity(1200, 45f);
                    break;
            }
        }
        public override float GetAttackDistance() => GetThrowingAttackDistance();
    }
}
