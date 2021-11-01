using UnityEngine;
/// <summary>
/// Contains parameters for movement and attack.
/// </summary>
namespace Core.Character.Patapon
{
    public class AttackMoveModel
    {
        public readonly string AnimationType;
        public readonly float MovingSpeed; //CALCULATED
        public readonly float AttackSpeedMultiplier; //NOT CALCULATED. ANIMATOR CALCULATES.
        private readonly PataponDistance _pataponDistance;
        private readonly DistanceCalculator _distanceCalculator;

        private readonly float _attackDistance;
        private readonly float _sizeOffset;
        private readonly float _groupOffset;
        public bool AlwaysAnimate { get; private set; }

        private float _totalOffset => _attackDistance + _sizeOffset;
        private float _rushAttackPosition => _pataponDistance.Front + PataponEnvironment.RushAttackDistance;

        /// <summary>
        /// Gets proper distance of current attack type. returns Infinity if the attack type isn't rush and there is no enemy on sight.
        /// </summary>
        public System.Func<float> GetRealDistance { get; private set; }

        /// <summary>
        /// Determines if it's attacking, defending or rushing.
        /// </summary>
        public AttackMoveType Type { get; }

        /// <summary>
        /// Creates the class that contains and calculates attack movement logic.
        /// </summary>
        /// <param name="animationType">Animation type in animator.</param>
        /// <param name="movingSpeed">The speed for moving character.</param>
        /// <param name="attackSpeed">The speed for animationg attack.</param>
        /// <param name="type">Attack movement type, which determines distance calculation logic.</param>
        /// <param name="attackDistance">Attack distance. e.g. melee unit is expected as 0.</param>
        internal AttackMoveModel(Patapon patapon, string animationType, AttackMoveType type, float movingSpeed, float attackSpeed, float attackDistance)
        {
            AnimationType = animationType;
            MovingSpeed = movingSpeed;
            AttackSpeedMultiplier = attackSpeed;
            _attackDistance = attackDistance;
            _pataponDistance = patapon.PataponDistance;
            _distanceCalculator = _pataponDistance.DistanceCalculator;
            _sizeOffset = patapon.PataponSize;
            _groupOffset = patapon.IndexInGroup * PataponEnvironment.AttackDistanceBetweenPatapons;

            SetAttackType(type);
            Type = type;
        }

        public float GetDistance() => GetRealDistance() - _groupOffset;
        public bool HasTarget() =>
             _pataponDistance.HasAttackTarget() && (Type == AttackMoveType.Attack && GetAttackPosition() <= _rushAttackPosition);
        public bool IsInAttackDistance()
        {
            return _pataponDistance.IsInTargetRange(GetDistance(), MovingSpeed * Time.deltaTime);
        }
        public AttackMoveModel SetAlwaysAnimate()
        {
            AlwaysAnimate = true;
            return this;
        }

        private void SetAttackType(AttackMoveType moveType)
        {
            switch (moveType)
            {
                case AttackMoveType.Attack:
                    GetRealDistance = GetAttackPosition;
                    break;
                case AttackMoveType.Defend:
                    GetRealDistance = GetDefendingPosition;
                    break;
                case AttackMoveType.Rush:
                    GetRealDistance = GetRushPosition;
                    AlwaysAnimate = true;
                    break;
            }
        }

        private float GetAttackPosition()
        {
            var closest = _distanceCalculator.GetClosest();
            if (closest.collider == null) return _pataponDistance.DefaultWorldPosition;
            return closest.point.x - _totalOffset;
        }
        private float GetDefendingPosition()
        {
            var closest = _distanceCalculator.GetClosest();
            var front = _pataponDistance.Front;
            if (closest.collider == null) return front;
            return Mathf.Min(front, closest.point.x - _totalOffset);
        }
        private float GetRushPosition()
        {
            var closest = _distanceCalculator.GetClosest();
            if (closest.collider == null) return _rushAttackPosition;
            return Mathf.Min(closest.point.x - _sizeOffset, _rushAttackPosition);
        }
    }
}
