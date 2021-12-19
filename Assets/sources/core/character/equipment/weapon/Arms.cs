using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class Arms : Weapon
    {
        private GameObject _copiedStone;
        private Transform _stoneTransform;
        private ArmTrigger[] _armTriggers;
        protected override float _throwMass => 1;
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
                    .Initialize(this, _color, _throwMass, _stoneTransform)
                    .Throw(650, 750, Vector3.zero);
            }
            else
            {
                foreach (var arm in _armTriggers)
                {
                    arm.EnableAttacking(Holder.Stat);
                }
            }
        }
        public override void StopAttacking()
        {
            foreach (var arm in _armTriggers)
            {
                arm.DisableAttacking();
            }
        }
        internal override void SetLastAttackCommandType(AttackCommandType attackCommandType)
        {
            base.SetLastAttackCommandType(attackCommandType);
            if (IsThrowing())
            {
                SetInitialVelocity(700, 68.80682f);
            }
        }
        public override float GetAttackDistance() => IsThrowing() ? GetThrowingAttackDistance() : 0;
        private bool IsThrowing()
        {
            if (Holder == null) return false;
            return LastAttackCommandType == AttackCommandType.ChargeAttack || (Holder.AttackTypeIndex == 1 && LastAttackCommandType == AttackCommandType.Attack);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Logic.DamageCalculator.DealDamage(
                Holder,
                Holder.Stat,
                collision.gameObject,
                collision.ClosestPoint(transform.position)
                );
        }
    }
}
