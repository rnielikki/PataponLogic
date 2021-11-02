using Core.Character.Patapon;
using UnityEngine;

namespace Core.Character
{
    class PataponAttackMoveData : IAttackMoveData
    {
        public float MaxRushAttackPosition => _distanceManager.Front + PataponEnvironment.RushAttackDistance;

        private readonly float _groupOffset;

        public float DefaultWorldPosition => _distanceManager.DefaultWorldPosition;

        private readonly PataponDistanceManager _distanceManager;
        private readonly DistanceCalculator _distanceCalculator;
        private readonly Patapon.Patapon _patapon;

        public PataponAttackMoveData(Patapon.Patapon patapon)
        {
            _patapon = patapon;
            _distanceManager = patapon.DistanceManager;
            _distanceCalculator = patapon.DistanceCalculator;
            _groupOffset = patapon.IndexInGroup * PataponEnvironment.AttackDistanceBetweenPatapons;
        }

        public float GetAttackPosition(float customDistance = -1)
        {
            if (customDistance < 0) customDistance = _patapon.AttackDistance;
            var closest = _distanceCalculator.GetClosest();
            if (closest.collider == null) return DefaultWorldPosition;
            return closest.point.x - customDistance - _patapon.CharacterSize - _groupOffset;
        }

        public float GetDefendingPosition(float customDistance = -1)
        {
            if (customDistance < 0) customDistance = _patapon.AttackDistance;
            var closest = _distanceCalculator.GetClosest();
            var front = _distanceManager.Front;
            if (closest.collider == null) return front - _groupOffset;
            return Mathf.Min(front, closest.point.x - -customDistance - _patapon.CharacterSize) - _groupOffset;
        }

        public float GetRushPosition()
        {
            var closest = _distanceCalculator.GetClosest();
            if (closest.collider == null) return MaxRushAttackPosition - _groupOffset;
            return Mathf.Min(closest.point.x - _patapon.CharacterSize, MaxRushAttackPosition) - _groupOffset;
        }
        public bool IsAttackableRange() => GetAttackPosition() <= MaxRushAttackPosition;
    }
}
