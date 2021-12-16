using UnityEngine;

namespace PataRoad.Core.Character
{
    class HazoronAttackMoveData : IAttackMoveData
    {
        public float MaxRushAttackPosition => _hazoron.DefaultWorldPosition - CharacterEnvironment.RushAttackDistance;

        private readonly Transform _pataponTransform;
        private readonly DistanceCalculator _distanceCalculator;
        private readonly Hazorons.Hazoron _hazoron;

        private float _min => _hazoron.DefaultWorldPosition - CharacterEnvironment.RushAttackDistance;
        private float _max => _hazoron.DefaultWorldPosition + CharacterEnvironment.RushAttackDistance;
        public bool WasHitLastTime { get; set; }
        public Vector2 LastHit { get; set; }

        public HazoronAttackMoveData(Hazorons.Hazoron hazoron)
        {
            _hazoron = hazoron;
            _distanceCalculator = hazoron.DistanceCalculator;
            _pataponTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        public float GetAttackPosition(float customDistance = -1)
        {
            if (customDistance < 0) customDistance = _hazoron.AttackDistance;
            var closest = _distanceCalculator.GetClosest(customDistance);
            if (closest == null) return _hazoron.DefaultWorldPosition;
            customDistance *= (1 - Mathf.InverseLerp(2, CharacterEnvironment.MaxYToScan, closest.Value.y));
            return Clamp(
                Mathf.Max(_pataponTransform.position.x + _hazoron.CharacterSize, closest.Value.x + customDistance + _hazoron.CharacterSize)
                );
        }

        public float GetDefendingPosition(float customDistance = -1)
        {
            return _hazoron.DefaultWorldPosition;
        }

        public float GetRushPosition()
        {
            var closest = _distanceCalculator.GetClosest(0);
            if (closest == null) return MaxRushAttackPosition;
            return Mathf.Max(closest.Value.x + _hazoron.CharacterSize, MaxRushAttackPosition);
        }

        public bool IsAttackableRange() => GetAttackPosition() >= MaxRushAttackPosition;
        private float Clamp(float value) => Mathf.Clamp(value, _min, _max);
    }
}
