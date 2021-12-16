using PataRoad.Core.Character.Patapons;
using UnityEngine;

namespace PataRoad.Core.Character
{
    class PataponAttackMoveData : IAttackMoveData
    {
        public float MaxRushAttackPosition => _distanceManager.Front + CharacterEnvironment.RushAttackDistance;

        private readonly float _groupOffset;

        private readonly PataponDistanceManager _distanceManager;
        private readonly DistanceCalculator _distanceCalculator;
        private readonly Patapon _patapon;
        private readonly Transform _pataponGroupTransform;
        private readonly Transform _pataponManagerTransform;

        private float _min => _pataponGroupTransform.position.x - CharacterEnvironment.RushAttackDistance;
        private float _max => _pataponManagerTransform.position.x + CharacterEnvironment.RushAttackDistance;
        public bool WasHitLastTime { get; set; }
        public Vector2 LastHit { get; set; }

        public PataponAttackMoveData(Patapon patapon)
        {
            _patapon = patapon;
            _pataponManagerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            _pataponGroupTransform = _patapon.GetComponentInParent<PataponGroup>().transform;
            _distanceManager = patapon.DistanceManager as PataponDistanceManager;
            _distanceCalculator = patapon.DistanceCalculator;
            _groupOffset = patapon.IndexInGroup * PataponEnvironment.AttackDistanceBetweenPatapons;
        }

        public float GetAttackPosition(float customDistance = -1)
        {
            if (customDistance < 0) customDistance = _patapon.AttackDistance;
            var closest = _distanceCalculator.GetClosest(customDistance);
            if (closest == null) return _patapon.DefaultWorldPosition;
            customDistance *= (1 - Mathf.InverseLerp(2, CharacterEnvironment.MaxYToScan, closest.Value.y));
            return Clamp(closest.Value.x - customDistance - _patapon.CharacterSize) - _groupOffset;
        }

        public float GetDefendingPosition(float customDistance = -1)
        {
            if (customDistance < 0) customDistance = _patapon.AttackDistance;
            var closest = _distanceCalculator.GetClosest(customDistance);
            var front = _distanceManager.Front;
            if (closest == null) return Clamp(front - _groupOffset);
            customDistance *= (1 - Mathf.InverseLerp(2, CharacterEnvironment.MaxYToScan, closest.Value.y));
            return Clamp(Mathf.Min(front, closest.Value.x - customDistance - _patapon.CharacterSize)) - _groupOffset;
        }

        public float GetRushPosition()
        {
            var closest = _distanceCalculator.GetClosest(0);
            if (closest == null) return MaxRushAttackPosition + _groupOffset;
            return Mathf.Min(closest.Value.x - _patapon.CharacterSize, MaxRushAttackPosition) + _groupOffset;
        }
        private float Clamp(float value) => Mathf.Clamp(value, _min, _max);
        public bool IsAttackableRange() => GetAttackPosition() <= MaxRushAttackPosition;
    }
}
