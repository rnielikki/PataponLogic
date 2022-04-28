using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class Spear : Weapon
    {
        private bool _isFeverAttack;
        public override bool IsTargetingCenter => true;

        private const float _feverAngle = 60;
        private void Start()
        {
            Init();
        }
        /// <summary>
        /// Throws spear, from CURRENT spear position and rotation FROM ANIMATION.
        /// </summary>
        public override void Attack(AttackCommandType attackCommandType)
        {
            var spearForThrowing = _objectPool.Get();
            float minVelocity, maxVelocity;
            if (attackCommandType == AttackCommandType.Defend)
            {
                minVelocity = 8;
                maxVelocity = 10;
            }
            else if (attackCommandType == AttackCommandType.FeverAttack)
            {
                minVelocity = 12 * Mathf.Cos(_feverAngle * Mathf.Deg2Rad);
                maxVelocity = 15 * Mathf.Cos(_feverAngle * Mathf.Deg2Rad);
            }
            else
            {
                minVelocity = 9;
                maxVelocity = 10;
            }

            spearForThrowing.GetComponent<WeaponInstance>()
                .Initialize(this, _material)
                .Throw(minVelocity / Time.fixedDeltaTime, maxVelocity / Time.fixedDeltaTime);
        }
        public override float AdjustAttackDistanceByYPosition(float attackDistance, float yDistance)
        {
            if (_isFeverAttack)
            {
                if (_initialVelocity.x == 0) return attackDistance / 2; //No zero division
                var yDiff = Mathf.Abs(7 - yDistance);
                return AdjustThrowingAttackDistanceByYPosition(attackDistance * (7 + yDiff) / 14, yDiff);
            }
            else
            {
                return AdjustThrowingAttackDistanceByYPosition(attackDistance, yDistance);
            }
        }
        internal override void SetLastAttackCommandType(AttackCommandType attackCommandType)
        {
            base.SetLastAttackCommandType(attackCommandType);
            _isFeverAttack = attackCommandType == AttackCommandType.FeverAttack;
            //CHANGE ANGLE IF CHANGE ANIMATION.
            switch (attackCommandType)
            {
                case AttackCommandType.Attack:
                    SetInitialVelocity(9f / Time.fixedDeltaTime, 49.586f);
                    break;
                case AttackCommandType.FeverAttack:
                    //readjust yaripon fever attack animation Y to: (Math.Pow(force*cos(angle as radian))/2g)
                    SetInitialVelocity(13.533f / Time.fixedDeltaTime, _feverAngle);
                    break;
                case AttackCommandType.Defend:
                    SetInitialVelocity(9f / Time.fixedDeltaTime, 45f);
                    break;
            }
        }
        public override float GetAttackDistance() => GetThrowingAttackDistance() / (_isFeverAttack ? 2 : 1);
    }
}
