using UnityEngine;

namespace PataRoad.Core.Character.Equipment.Weapon
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
        private static readonly Vector3 _throwAdditionalForce = Vector3.up * 0.75f;
        public override float AttackDistanceOffset => 10 * Map.Weather.WeatherInfo.Wind?.Magnitude ?? 0;
        private void Start()
        {
            _minAttackDistance = 20;
            AttackDistanceOffset = 18 * Map.Weather.WeatherInfo.Wind.Magnitude;
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
                minForce = 5;
                maxForce = 8;
            }
            else
            {
                minForce = 14;
                maxForce = 17.5f;
            }

            arrowForThrowing.GetComponent<WeaponInstance>()
                .Initialize(this, 0.02f, _arrowTransform)
                .Throw(minForce, maxForce, _throwAdditionalForce);
        }
    }
}
