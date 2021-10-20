using UnityEngine;

namespace Core.Character.Equipment.Weapon
{
    class Arms : WeaponObject
    {
        private GameObject _copiedStone;
        private Sprite _stoneSprite;
        private Transform _stoneTransform;
        private void Awake()
        {
            Init();
            _copiedStone = GetWeaponInstance();
            _stoneTransform = transform.Find("Stone");
            _stoneSprite = _stoneTransform.GetComponent<SpriteRenderer>().sprite;
        }
        public override void Attack(AttackType attackType)
        {
            if (attackType == AttackType.ChargeAttack)
            {
                var stoneForThrowing = Instantiate(_copiedStone, transform.root.parent);
                stoneForThrowing.GetComponent<SpriteRenderer>().sprite = _stoneSprite;
                stoneForThrowing.transform.position = _stoneTransform.position;
                stoneForThrowing.transform.rotation = _stoneTransform.rotation;
                stoneForThrowing.GetComponent<WeaponInstance>().Throw(0.75f);
            }
        }
    }
}
