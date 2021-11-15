using UnityEngine;

namespace PataRoad.Core.Character
{
    class HazoronAttackMoveData : IAttackMoveData
    {
        public float MaxRushAttackPosition => DefaultWorldPosition - CharacterEnvironment.RushAttackDistance;

        public float DefaultWorldPosition { get; }
        private readonly Transform _pataponTransform;
        private readonly DistanceCalculator _distanceCalculator;
        private readonly Hazorons.Hazoron _hazoron;

        private float _min => DefaultWorldPosition - CharacterEnvironment.RushAttackDistance;
        private float _max => DefaultWorldPosition + CharacterEnvironment.RushAttackDistance;
        public bool WasHitLastTime { get; set; }
        public Vector2 LastHit { get; set; }

        public HazoronAttackMoveData(Hazorons.Hazoron hazoron)
        {
            _hazoron = hazoron;
            _distanceCalculator = hazoron.DistanceCalculator;
            _pataponTransform = GameObject.FindGameObjectWithTag("Player").transform;
            DefaultWorldPosition = hazoron.transform.position.x;
        }
        public float GetAttackPosition(float customDistance = -1)
        {
            if (customDistance < 0) customDistance = _hazoron.AttackDistance;
            var closest = _distanceCalculator.GetClosest();
            if (closest == null) return DefaultWorldPosition;
            customDistance *= (1 - Mathf.InverseLerp(2, CharacterEnvironment.MaxYToScan, closest.Value.y));
            return Clamp(
                Mathf.Max(_pataponTransform.position.x + _hazoron.CharacterSize, closest.Value.x + customDistance + _hazoron.CharacterSize)
                );
        }

        public float GetDefendingPosition(float customDistance = -1)
        {
            return DefaultWorldPosition;
        }

        public float GetRushPosition()
        {
            var closest = _distanceCalculator.GetClosest();
            if (closest == null) return MaxRushAttackPosition;
            return Mathf.Max(closest.Value.x + _hazoron.CharacterSize, MaxRushAttackPosition);
        }

        public bool IsAttackableRange() => GetAttackPosition() >= MaxRushAttackPosition;
        private float Clamp(float value) => Mathf.Clamp(value, _min, _max);
    }
}
