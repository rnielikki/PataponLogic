using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class Spear : Weapon
    {
        /// <summary>
        /// copied spear for throwing.
        /// </summary>
        private GameObject _copiedSpear;
        private bool _isFeverAttack;

        private const float _feverAngle = 60;
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
                minVelocity = 10;
                maxVelocity = 12;
            }

            spearForThrowing.GetComponent<WeaponInstance>()
                .Initialize(this, _color)
                .Throw(minVelocity / Time.fixedDeltaTime, maxVelocity / Time.fixedDeltaTime);
        }
        public override float AdjustAttackDistanceByYPosition(float attackDistance, float yDistance)
        {
            if (_isFeverAttack)
            {
                if (_initialVelocity.x == 0) return attackDistance / 2; //No zero division
                var velocityRate = _initialVelocity.y / _initialVelocity.x;
                return (Mathf.Sqrt((yDistance + 0.25f * velocityRate * Mathf.Pow(attackDistance, 2)) / velocityRate) + 0.5f * attackDistance) / 2;
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
                    SetInitialVelocity(11f / Time.fixedDeltaTime, 49.586f);
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
        public override float GetAttackDistance() => GetThrowingAttackDistance();
    }
}
