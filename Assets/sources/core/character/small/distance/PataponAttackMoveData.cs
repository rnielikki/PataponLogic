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

        private float _min => _pataponGroupTransform.position.x - CharacterEnvironment.MaxAttackDistance;
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

        public float GetAttackPosition()
        {
            return Clamp(GetAttackPositionNonClamp()) - _groupOffset;
        }

        public float GetDefendingPosition()
        {
            var attackDistance = _patapon.AttackDistance;
            var closest = _distanceCalculator.GetClosest();
            var front = _distanceManager.Front;
            if (closest == null) return Clamp(front - _groupOffset);
            attackDistance = _patapon.Weapon.AdjustAttackDistanceByYPosition(attackDistance, closest.Value.y - _patapon.RootTransform.position.y);
            return Clamp(Mathf.Min(front, closest.Value.x - attackDistance - _patapon.CharacterSize)) - _groupOffset;
        }

        public float GetRushPosition()
        {
            var closest = _distanceCalculator.GetClosest();
            if (closest == null) return MaxRushAttackPosition + _groupOffset;
            return Mathf.Min(closest.Value.x - _patapon.CharacterSize, MaxRushAttackPosition) + _groupOffset;
        }
        private float Clamp(float value) => Mathf.Clamp(value, _min, _max);
        private float GetAttackPositionNonClamp()
        {
            var attackDistance = _patapon.AttackDistance;

            var closest = _distanceCalculator.GetClosest();
            if (closest == null) return _patapon.DefaultWorldPosition;
            attackDistance = _patapon.Weapon.AdjustAttackDistanceByYPosition(attackDistance, closest.Value.y - _patapon.RootTransform.position.y);
            return closest.Value.x - attackDistance - _patapon.CharacterSize;
        }
        public bool IsAttackableRange() => GetAttackPositionNonClamp() - _groupOffset <= MaxRushAttackPosition;
    }
}
