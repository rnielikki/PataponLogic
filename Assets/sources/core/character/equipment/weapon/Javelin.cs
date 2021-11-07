using UnityEngine;

namespace PataRoad.Core.Character.Equipment.Weapon
{
    class Javelin : WeaponObject
    {
        /// <summary>
        /// copied spear for throwing.
        /// </summary>
        private GameObject _copiedJavelin;
        protected override float _minAttackDistance { get; set; } = 0.5f;
        public override float AttackDistanceOffset => Map.Weather.WeatherInfo.Wind?.Magnitude ?? 0;

        private void Start()
        {
            Init();
            _copiedJavelin = GetWeaponInstance();
        }
        /// <summary>
        /// Throws spear, from CURRENT spear position and rotation FROM ANIMATION.
        /// </summary>
        public override void Attack(AttackCommandType attackCommandType)
        {
            switch (attackCommandType)
            {
                case AttackCommandType.Attack:
                    ThrowWeaponInstance(125, 175);
                    break;
                case AttackCommandType.FeverAttack:
                    ThrowWeaponInstance(150, 180, -15);
                    ThrowWeaponInstance(180, 200);
                    ThrowWeaponInstance(150, 180, 15);
                    break;
                case AttackCommandType.Defend:
                    ThrowWeaponInstance(100, 110);
                    break;
            }
            void ThrowWeaponInstance(float minForce, float maxForce, int angle = 0)
            {
                var instance = Instantiate(_copiedJavelin, transform.root.parent);
                if (angle != 0) instance.transform.Rotate(Vector3.forward * angle);
                instance.GetComponent<WeaponInstance>()
                    .Initialize(this, 0.2f)
                    .Throw(minForce, maxForce, Vector3.zero);
            }
        }
    }
}
