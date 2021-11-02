using UnityEngine;

namespace Core.Character
{
    /// <summary>
    /// Calculates distance and helps to move ANY characters.
    /// </summary>
    public class DistanceCalculator
    {
        private readonly GameObject _target;
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
        internal DistanceCalculator(GameObject target, float sight, int layerMask, Vector2 direction, float allowedRange)
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
        /// <param name="allowedRange">Allows to pass as "In range". This is important for range units.</param>
        internal static DistanceCalculator GetPataponDistanceCalculator(GameObject target, float range) =>
            new DistanceCalculator(target, Patapon.PataponEnvironment.PataponSight, LayerMask.GetMask("structures", "enemies"), Vector2.right, range);
        /// <summary>
        /// <see cref="DistanceCalculator"/> for Hazoron (also from right to left).
        /// </summary>
        /// <param name="target">The target game object. ("from")</param>
        /// <param name="allowedRange">Allows to pass as "In range". This is important for range units.</param>
        internal static DistanceCalculator GetHazoronDistanceCalculator(GameObject target, float range) =>
            new DistanceCalculator(target, Patapon.PataponEnvironment.PataponSight, LayerMask.GetMask("patapons"), Vector2.left, range);
        /// <summary>
        /// Shoots RayCast to closest structure or enemy and returns the raycast hit if found.
        /// </summary>
        /// <returns><see cref="RaycastHit2D"/> of the closest structure or enemy.</returns>
        public RaycastHit2D GetClosest() => Physics2D.Raycast(_target.transform.position, _direction, _sight, _layerMask);
        public bool IsInTargetRange(float targetX, float offset) => IsInTargetRange(_target.transform.position.x, targetX, offset);
        public bool IsInTargetRange(float x, float targetX, float offset) => targetX - (offset + _allowedRange) < x && x < targetX + (offset + _allowedRange);
        /// <summary>
        /// Check if the character has attack target on their sight.
        /// </summary>
        /// <returns><c>true</c> if Patapon finds obstacle (attack) target to Patapon sight, otherwise <c>false</c>.</returns>
        public bool HasAttackTarget() => GetClosest().collider != null;
    }
}
