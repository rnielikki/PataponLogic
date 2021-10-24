using UnityEngine;

namespace Core.Character
{
    /// <summary>
    /// Calculates distance and helps to move Patapons or Patapon groups
    /// </summary>
    internal class DistanceCalculator
    {
        private readonly GameObject _target;
        private readonly float _sight;
        private readonly int _layerMask;
        private readonly Vector2 _direction;
        /// <summary>
        /// Constructor for getting distances from target game object, like Patapon-Enemy, Enemy-Patapon, Patapon-Structure etc.
        /// </summary>
        /// <param name="target">The target game object. ("from")</param>
        /// <param name="sight">Maximum sight of the target. This is equivalent to raycast distance.</param>
        /// <param name="layerMask">Masks of layers to detect. ("to") Get this value using <see cref="LayerMask"/>.</param>
        internal DistanceCalculator(GameObject target, float sight, int layerMask, Vector2 direction)
        {
            _target = target;
            _sight = sight;
            _direction = direction.normalized;
            _layerMask = layerMask;
        }
        /// <summary>
        /// <see cref="DistanceCalculator"/> for Patapon.
        /// </summary>
        /// <param name="target">The target game object. ("from")</param>
        internal static DistanceCalculator GetPataponDistanceCalculator(GameObject target) => new DistanceCalculator(target, Patapon.PataponEnvironment.PataponSight, LayerMask.GetMask("structures", "enemies"), Vector2.right);
        /// <summary>
        /// Shoots RayCast to closest structure or enemy and returns the raycast hit if found.
        /// </summary>
        /// <returns><see cref="RaycastHit2D"/> of the closest structure or enemy.</returns>
        public RaycastHit2D GetClosest() => Physics2D.Raycast(_target.transform.position, _direction, _sight, _layerMask);
    }
}
