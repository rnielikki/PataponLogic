using UnityEngine;

namespace Core.Character.Equipment.Weapon
{
    class Javelin : WeaponObject
    {
        /// <summary>
        /// copied spear for throwing.
        /// </summary>
        private GameObject _copiedJavelin;
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
                    ThrowWeaponInstance(1.5f);
                    break;
                case AttackCommandType.FeverAttack:
                    ThrowWeaponInstance(1.5f, -15);
                    ThrowWeaponInstance(1.75f);
                    ThrowWeaponInstance(1.5f, 15);
                    break;
                case AttackCommandType.Defend:
                    ThrowWeaponInstance(1);
                    break;
            }
            void ThrowWeaponInstance(float force, int angle = 0)
            {
                var instance = Instantiate(_copiedJavelin, transform.root.parent);
                if (angle != 0) instance.transform.Rotate(Vector3.forward * angle);
                instance.GetComponent<WeaponInstance>()
                    .Initialize(this)
                    .Throw(force);
            }
        }
    }
}
