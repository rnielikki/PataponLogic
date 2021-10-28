using UnityEngine;

namespace Core.Character.Equipment.Weapon
{
    class Javelin : WeaponObject
    {
        /// <summary>
        /// copied spear for throwing.
        /// </summary>
        private GameObject _copiedJavelin;
        private Sprite _sprite;
        private void Awake()
        {
            Init();
            _copiedJavelin = Resources.Load("Characters/Equipments/PrefabBase/WeaponInstance") as GameObject;
            _copiedJavelin.layer = gameObject.layer;
            _sprite = GetComponent<SpriteRenderer>().sprite;
        }
        /// <summary>
        /// Throws spear, from CURRENT spear position and rotation FROM ANIMATION.
        /// </summary>
        public override void Attack(AttackCommandType attackCommandType)
        {
            switch (attackCommandType)
            {
                case AttackCommandType.Attack:
                    ThrowWeaponInstance(0.75f);
                    break;
                case AttackCommandType.FeverAttack:
                    ThrowWeaponInstance(0.7f, -15);
                    ThrowWeaponInstance(0.75f);
                    ThrowWeaponInstance(0.7f, 15);
                    break;
                case AttackCommandType.Defend:
                    ThrowWeaponInstance(0.5f);
                    break;
            }
            void ThrowWeaponInstance(float force, int angle = 0)
            {
                var instance = Instantiate(_copiedJavelin, transform.root.parent);
                instance.transform.position = transform.position;
                instance.transform.rotation = transform.rotation;
                if (angle != 0) instance.transform.Rotate(Vector3.forward * angle);
                instance.GetComponent<WeaponInstance>()
                    .SetSprite(_sprite)
                    .Throw(force);
            }
        }
    }
}
