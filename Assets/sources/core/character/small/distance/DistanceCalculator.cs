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

        /// <summary>
        /// Constructor for getting distances from target game object, like Patapon-Enemy, Enemy-Patapon, Patapon-Structure etc.
        /// </summary>
        /// <param name="target">The target game object. ("from")</param>
        /// <param name="sight">Maximum sight of the target. This is equivalent to raycast distance.</param>
        /// <param name="layerMask">Masks of layers to detect. ("to") Get this value using <see cref="LayerMask"/>.</param>
        /// <param name="allowedRange">Allows to pass as "In range". This is important for range units.</param>
        internal DistanceCalculator(SmallCharacter target, float sight, int layerMask, Vector2 direction)
        {
            _target = target;
            _sight = sight;
            _direction = direction.normalized;
            _layerMask = layerMask;
        }
        /// <summary>
        /// <see cref="DistanceCalculator"/> for Patapon (also from left to right).
        /// </summary>
        /// <param name="target">The target game object. ("from")</param>
        /// <param name="range">Allows to pass as "In range". This is important for range units.</param>
        internal static DistanceCalculator GetPataponDistanceCalculator(Patapon.Patapon target) =>
            new DistanceCalculator(target, Patapon.PataponEnvironment.PataponSight, LayerMask.GetMask("structures", "enemies"), Vector2.right);
        /// <summary>
        /// <see cref="DistanceCalculator"/> for Hazoron (also from right to left).
        /// </summary>
        /// <param name="target">The target game object. ("from")</param>
        /// <param name="range">Allows to pass as "In range". This is important for range units.</param>
        internal static DistanceCalculator GetHazoronDistanceCalculator(Hazoron.Hazoron target) =>
            new DistanceCalculator(target, Patapon.PataponEnvironment.PataponSight, LayerMask.GetMask("patapons"), Vector2.left);


        //boxcast data
        private static Vector2 _boxSize = new Vector2(0.001f, Patapon.PataponEnvironment.MaxYToScan); //size for boxcasting. NOTE: boxcast doesn't catch from initial box position.
        private static Vector2 _boxcastYOffset = (_boxSize.y / 2) * Vector2.up;
        private static float _boxcastXOffset = _boxSize.x * 0.6f;

        /// <summary>
        /// Shoots RayCast to closest structure or enemy and returns the raycast hit if found.
        /// </summary>
        /// <returns>X position as collider hit, Y position as collided game object position.</returns>
        public Vector2? GetClosest() => GetClosest((Vector2)_target.transform.position + _target.AttackDistance * _direction);

        private Vector2? GetClosest(Vector2 castPoint)//bidirectional
        {
            var raycast = Physics2D.BoxCast(castPoint + _boxcastXOffset * _direction + _boxcastYOffset, _boxSize, 0, -_direction, _target.AttackDistance, _layerMask);
            var p = ReturnInRange(raycast);
            if (p == null)
            {
                raycast = Physics2D.BoxCast(castPoint - _boxcastXOffset * _direction + _boxcastYOffset, _boxSize, 0, _direction, _sight - _target.AttackDistance, _layerMask);
                p = ReturnInRange(raycast);
                if (p == null) return null;
            }
            return new Vector2(p.Value, raycast.transform.position.y);

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
        /// <note>This is alternative to "continuous" collider (which can cause performance problem)</note>
        public float GetSafeForwardPosition(float input)
        {
            var raycast = Physics2D.BoxCast((Vector2)_target.transform.position - _boxSize.x * _direction + _boxcastYOffset, _boxSize, 0, _direction, _sight, _layerMask);
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
        public bool IsInTargetRange(float x, float targetX, float offset) => targetX - offset < x && x < targetX + offset;
        /// <summary>
        /// Check if the character has attack target on their sight.
        /// </summary>
        /// <returns><c>true</c> if Patapon finds obstacle (attack) target to Patapon sight, otherwise <c>false</c>.</returns>
        public bool HasAttackTarget() => GetClosest() != null;
    }
}
