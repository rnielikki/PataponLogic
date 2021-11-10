using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class Javelin : Weapon
    {
        /// <summary>
        /// copied spear for throwing.
        /// </summary>
        private GameObject _copiedJavelin;
        public override float MinAttackDistance { get; } = 0.5f;
        public override float WindAttackDistanceOffset { get; } = 1;

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
                    ThrowWeaponInstance(625, 875);
                    break;
                case AttackCommandType.FeverAttack:
                    ThrowWeaponInstance(750, 900, -15);
                    ThrowWeaponInstance(900, 1000);
                    ThrowWeaponInstance(750, 900, 15);
                    break;
                case AttackCommandType.Defend:
                    ThrowWeaponInstance(500, 550);
                    break;
            }
            void ThrowWeaponInstance(float minForce, float maxForce, int angle = 0)
            {
                var instance = Instantiate(_copiedJavelin, transform.root.parent);
                if (angle != 0) instance.transform.Rotate(Vector3.forward * angle);
                instance.GetComponent<WeaponInstance>()
                    .Initialize(this)
                    .Throw(minForce, maxForce);
            }
        }
    }
}
