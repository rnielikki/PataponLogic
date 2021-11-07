using UnityEngine;

namespace PataRoad.Core.Character.Equipment.Weapon
{
    class Spear : WeaponObject
    {
        /// <summary>
        /// copied spear for throwing.
        /// </summary>
        private GameObject _copiedSpear;
        private static readonly Vector3 _throwAdditionalForce = Vector3.up * 0.1f;
        public override float MinAttackDistance { get; } = 8.5f;
        public override float WindAttackDistanceOffset { get; } = 5;

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
                minForce = 30;
                maxForce = 60;
            }
            else
            {
                minForce = 80;
                maxForce = 95;
            }

            spearForThrowing.GetComponent<WeaponInstance>()
                .Initialize(this)
                .Throw(minForce, maxForce, _throwAdditionalForce);
        }
    }
}
