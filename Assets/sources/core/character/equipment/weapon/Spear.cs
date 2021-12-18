using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class Spear : Weapon
    {
        /// <summary>
        /// copied spear for throwing.
        /// </summary>
        private GameObject _copiedSpear;
        private void Start()
        {
            Init();
            _copiedSpear = GetWeaponInstance();
        }
        /// <summary>
        /// Throws spear, from CURRENT spear position and rotation FROM ANIMATION.
        /// </summary>
        public override void Attack(AttackCommandType attackCommandType)
        {
            var spearForThrowing = Instantiate(_copiedSpear, transform.root.parent);
            float minForce, maxForce;
            if (attackCommandType == AttackCommandType.Defend)
            {
                minForce = 300;
                maxForce = 600;
            }
            else
            {
                minForce = 800;
                maxForce = 950;
            }

            spearForThrowing.GetComponent<WeaponInstance>()
                .Initialize(this)
                .Throw(minForce, maxForce);
        }
        internal override void SetLastAttackCommandType(AttackCommandType attackCommandType)
        {
            base.SetLastAttackCommandType(attackCommandType);
            //CHANGE ANGLE IF CHANGE ANIMATION.
            switch (attackCommandType)
            {
                case AttackCommandType.Attack:
                    SetInitialVelocity(875, 49.586f);
                    break;
                case AttackCommandType.FeverAttack:
                    SetInitialVelocity(875, 50.807f);
                    break;
                case AttackCommandType.Defend:
                    SetInitialVelocity(450, 36.918f);
                    break;
            }
        }
        public override float GetAttackDistance() => GetThrowingAttackDistance();

    }
}
