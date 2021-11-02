using UnityEngine;

namespace Core.Character
{
    class HazoronAttackMoveData : IAttackMoveData
    {
        public float MaxRushAttackPosition => DefaultWorldPosition - Patapon.PataponEnvironment.RushAttackDistance;

        public float DefaultWorldPosition { get; }
        private readonly Transform _pataponTransform;
        private readonly DistanceCalculator _distanceCalculator;
        private readonly Hazoron.Hazoron _hazoron;

        public HazoronAttackMoveData(Hazoron.Hazoron hazoron)
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
            return Mathf.Max(_pataponTransform.position.x, closest.Value + customDistance + _hazoron.CharacterSize);
        }

        public float GetDefendingPosition(float customDistance = -1)
        {
            return DefaultWorldPosition;
        }

        public float GetRushPosition()
        {
            var closest = _distanceCalculator.GetClosest();
            if (closest == null) return MaxRushAttackPosition;
            return Mathf.Max(closest.Value + _hazoron.CharacterSize, MaxRushAttackPosition);
        }

        public bool IsAttackableRange() => GetAttackPosition() >= MaxRushAttackPosition;
    }
}
