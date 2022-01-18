using UnityEngine;

namespace PataRoad.Core.Character
{
    class HazoronAttackMoveData : IAttackMoveData
    {
        public float MaxRushAttackPosition => _hazoron.DefaultWorldPosition - CharacterEnvironment.RushAttackDistance;

        private readonly DistanceCalculator _distanceCalculator;
        private readonly Hazorons.Hazoron _hazoron;

        private float _min => _hazoron.DefaultWorldPosition - CharacterEnvironment.Sight;
        private float _max => _hazoron.DefaultWorldPosition + CharacterEnvironment.MaxAttackDistance;
        public bool WasHitLastTime { get; set; }
        public Vector2 LastHit { get; set; }

        public HazoronAttackMoveData(Hazorons.Hazoron hazoron)
        {
            _hazoron = hazoron;
            _distanceCalculator = hazoron.DistanceCalculator;
        }
        public float GetAttackPosition()
        {
            return Clamp(GetAttackPositionNonClamp());
        }

        public float GetDefendingPosition()
        {
            return _hazoron.DefaultWorldPosition;
        }

        public float GetRushPosition()
        {
            var closest = _distanceCalculator.GetClosestForAttack();
            if (closest == null) return MaxRushAttackPosition;
            return Mathf.Max(closest.Value.x + _hazoron.CharacterSize, MaxRushAttackPosition);
        }

        public bool IsAttackableRange() => GetAttackPositionNonClamp() >= MaxRushAttackPosition;
        private float GetAttackPositionNonClamp()
        {
            var attackDistance = _hazoron.AttackDistance;
            if (attackDistance < 0) attackDistance = _hazoron.AttackDistance;
            var closest = _distanceCalculator.GetClosestForAttack();
            if (closest == null) return _hazoron.DefaultWorldPosition;
            attackDistance = _hazoron.Weapon.AdjustAttackDistanceByYPosition(attackDistance, closest.Value.y);

            return closest.Value.x + attackDistance + _hazoron.CharacterSize;
        }
        private float Clamp(float value) => Mathf.Clamp(value, _min, _max);
    }
}
