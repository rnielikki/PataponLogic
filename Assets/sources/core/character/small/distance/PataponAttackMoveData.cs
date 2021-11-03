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
        private readonly Transform _pataponGroupTransform;
        private readonly Transform _pataponManagerTransform;

        private float _min => _pataponGroupTransform.position.x - PataponEnvironment.PataponSight;
        private float _max => _pataponManagerTransform.position.x + PataponEnvironment.RushAttackDistance;

        public PataponAttackMoveData(Patapon.Patapon patapon)
        {
            _patapon = patapon;
            _pataponManagerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            _pataponGroupTransform = _patapon.GetComponentInParent<PataponGroup>().transform;
            _distanceManager = patapon.DistanceManager;
            _distanceCalculator = patapon.DistanceCalculator;
            _groupOffset = patapon.IndexInGroup * PataponEnvironment.AttackDistanceBetweenPatapons;
        }

        public float GetAttackPosition(float customDistance = -1)
        {
            if (customDistance < 0) customDistance = _patapon.AttackDistance;
            var closest = _distanceCalculator.GetClosest();
            if (closest == null) return DefaultWorldPosition;
            customDistance *= (1 - Mathf.InverseLerp(0, PataponEnvironment.MaxYToScan, closest.Value.y));
            return Clamp(closest.Value.x - customDistance - _patapon.CharacterSize) - _groupOffset;
        }

        public float GetDefendingPosition(float customDistance = -1)
        {
            if (customDistance < 0) customDistance = _patapon.AttackDistance;
            var closest = _distanceCalculator.GetClosest();
            var front = _distanceManager.Front;
            if (closest == null) return Clamp(front - _groupOffset);
            customDistance *= (1 - Mathf.InverseLerp(0, PataponEnvironment.MaxYToScan, closest.Value.y));
            return Clamp(Mathf.Min(front, closest.Value.x - customDistance - _patapon.CharacterSize)) - _groupOffset;
        }

        public float GetRushPosition()
        {
            var closest = _distanceCalculator.GetClosest();
            if (closest == null) return MaxRushAttackPosition - _groupOffset;
            return Mathf.Min(closest.Value.x - _patapon.CharacterSize, MaxRushAttackPosition) - _groupOffset;
        }
        private float Clamp(float value) => Mathf.Clamp(value, _min, _max);
        public bool IsAttackableRange() => GetAttackPosition() <= MaxRushAttackPosition;
    }
}
