using UnityEngine;

namespace Core.Character.Equipment.Weapon
{
    class Arms : WeaponObject
    {
        private GameObject _copiedStone;
        private Transform _stoneTransform;
        private ArmTrigger[] _armTriggers;
        private void Start()
        {
            _stoneTransform = transform.Find("Stone");
            Init();
            _copiedStone = GetWeaponInstance();
            _armTriggers = GetComponentsInChildren<ArmTrigger>();
        }
        protected override Sprite GetThrowableWeaponSprite() => _stoneTransform.GetComponent<SpriteRenderer>().sprite;
        public override void Attack(AttackCommandType attackCommandType)
        {
            if (attackCommandType == AttackCommandType.ChargeAttack)
            {
                var stoneForThrowing = Instantiate(_copiedStone, transform.root.parent);
                stoneForThrowing.GetComponent<WeaponInstance>()
                    .Initialize(this, _stoneTransform)
                    .Throw(1);
            }
            else
            {
                foreach (var arm in _armTriggers)
                {
                    arm.EnableCollider();
                }
            }
        }
        public override void StopAttacking()
        {
            foreach (var arm in _armTriggers)
            {
                arm.DisableCollider();
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Logic.DamageCalculator.DealDamage(
                Holder,
                collision.gameObject,
                collision.ClosestPoint(transform.position)
                );
        }
    }
}
