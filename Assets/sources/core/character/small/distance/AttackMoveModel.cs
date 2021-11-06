using UnityEngine;
/// <summary>
/// Contains parameters for movement and attack.
/// </summary>
namespace PataRoad.Core.Character
{
    public class AttackMoveModel
    {
        public readonly string AnimationType;
        public readonly float MovingSpeed; //CALCULATED
        public readonly float AttackSpeedMultiplier; //NOT CALCULATED. ANIMATOR CALCULATES.
        private readonly DistanceCalculator _distanceCalculator;

        public bool AlwaysAnimate { get; private set; }

        /// <summary>
        /// Gets proper distance of current attack type. returns Infinity if the attack type isn't rush and there is no enemy on sight.
        /// </summary>
        public System.Func<float, float> GeAttackPositioneFromData { get; private set; }

        /// <summary>
        /// Determines if it's attacking, defending or rushing.
        /// </summary>
        public AttackMoveType Type { get; }

        private readonly IAttackMoveData _data;
        private readonly float _attackDistance;

        /// <summary>
        /// Creates the class that contains and calculates attack movement logic.
        /// </summary>
        /// <param name="animationType">Animation type in animator.</param>
        /// <param name="movingSpeed">The speed for moving character.</param>
        /// <param name="attackSpeed">The speed for animationg attack.</param>
        /// <param name="type">Attack movement type, which determines distance calculation logic.</param>
        /// <param name="attackDistance">Distance from enemy while attacking. -1 is default, which will use calculated distance from <see cref="Patapons"/>.</param>
        internal AttackMoveModel(SmallCharacter character, string animationType, AttackMoveType type, float movingSpeed, float attackSpeed, float attackDistance = -1)
        {
            AnimationType = animationType;
            MovingSpeed = movingSpeed;
            AttackSpeedMultiplier = attackSpeed;
            _data = character.AttackMoveData;
            _attackDistance = attackDistance;

            _distanceCalculator = character.DistanceCalculator;

            SetAttackType(type);
            Type = type;
        }

        public bool HasTarget() =>
             _distanceCalculator.HasAttackTarget() && (Type == AttackMoveType.Attack && _data.IsAttackableRange());
        public bool IsInAttackDistance()
        {
            return _distanceCalculator.IsInTargetRange(GetPosition(), MovingSpeed * Time.deltaTime);
        }
        public float GetPosition() => GeAttackPositioneFromData(_attackDistance);
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
                    GeAttackPositioneFromData = _data.GetAttackPosition;
                    break;
                case AttackMoveType.Defend:
                    GeAttackPositioneFromData = _data.GetDefendingPosition;
                    break;
                case AttackMoveType.Rush:
                    GeAttackPositioneFromData = (_) => _data.GetRushPosition();
                    AlwaysAnimate = true;
                    break;
            }
        }
    }
}
