using UnityEngine;

namespace Core.Character
{
    /// <summary>
    /// Calculates distance and helps to move ANY characters.
    /// </summary>
    public class DistanceCalculator
    {
        private readonly SmallCharacter _target;
        private readonly float _sight;
        private readonly int _layerMask;
        private readonly Vector2 _direction;
        private readonly float _allowedRange;
        /// <summary>
        /// Constructor for getting distances from target game object, like Patapon-Enemy, Enemy-Patapon, Patapon-Structure etc.
        /// </summary>
        /// <param name="target">The target game object. ("from")</param>
        /// <param name="sight">Maximum sight of the target. This is equivalent to raycast distance.</param>
        /// <param name="layerMask">Masks of layers to detect. ("to") Get this value using <see cref="LayerMask"/>.</param>
        /// <param name="allowedRange">Allows to pass as "In range". This is important for range units.</param>
        internal DistanceCalculator(SmallCharacter target, float sight, int layerMask, Vector2 direction, float allowedRange)
        {
            _target = target;
            _sight = sight;
            _direction = direction.normalized;
            _layerMask = layerMask;
            _allowedRange = allowedRange;
        }
        /// <summary>
        /// <see cref="DistanceCalculator"/> for Patapon (also from left to right).
        /// </summary>
        /// <param name="target">The target game object. ("from")</param>
        /// <param name="range">Allows to pass as "In range". This is important for range units.</param>
        internal static DistanceCalculator GetPataponDistanceCalculator(Patapon.Patapon target, float range) =>
            new DistanceCalculator(target, Patapon.PataponEnvironment.PataponSight, LayerMask.GetMask("structures", "enemies"), Vector2.right, range);
        /// <summary>
        /// <see cref="DistanceCalculator"/> for Hazoron (also from right to left).
        /// </summary>
        /// <param name="target">The target game object. ("from")</param>
        /// <param name="range">Allows to pass as "In range". This is important for range units.</param>
        internal static DistanceCalculator GetHazoronDistanceCalculator(Hazoron.Hazoron target, float range) =>
            new DistanceCalculator(target, Patapon.PataponEnvironment.PataponSight, LayerMask.GetMask("patapons"), Vector2.left, range);
        /// <summary>
        /// Shoots RayCast to closest structure or enemy and returns the raycast hit if found.
        /// </summary>
        /// <returns><see cref="RaycastHit2D"/> of the closest structure or enemy.</returns>
        public float? GetClosest()
        {
            Vector2 castPoint = (Vector2)_target.transform.position + _target.AttackDistance * _direction;
            var raycast = Physics2D.Raycast(castPoint, -_direction, _sight, _layerMask);
            var p = ReturnInRange(raycast);
            if (p == null)
            {
                raycast = Physics2D.Raycast(castPoint, _direction, _sight, _layerMask);
                p = ReturnInRange(raycast);
            }
            return p;

            float? ReturnInRange(RaycastHit2D hit)
            {
                if (hit.collider == null) return null;
                return hit.point.x;
            }
        }

        /// <summary>
        /// Prevents "going through collider".
        /// </summary>
        /// <returns>Position of close sight. If there's no target in close sight, input value.</returns>
        public float GetSafeForwardPosition(float input)
        {
            var raycast = Physics2D.Raycast(_target.transform.position, _direction, _sight, _layerMask);
            if (raycast.collider == null)
            {
                return input;
            }
            var xDir = _direction.x;
            if (xDir < 0)
            {
                return Mathf.Max(raycast.point.x, input);
            }
            else
            {
                return Mathf.Min(raycast.point.x, input);
            }
        }

        public bool IsInTargetRange(float targetX, float offset) => IsInTargetRange(_target.transform.position.x, targetX, offset);
        public bool IsInTargetRange(float x, float targetX, float offset) => targetX - (offset + _allowedRange) < x && x < targetX + (offset + _allowedRange);
        /// <summary>
        /// Check if the character has attack target on their sight.
        /// </summary>
        /// <returns><c>true</c> if Patapon finds obstacle (attack) target to Patapon sight, otherwise <c>false</c>.</returns>
        public bool HasAttackTarget() => GetClosest() != null;
    }
}
