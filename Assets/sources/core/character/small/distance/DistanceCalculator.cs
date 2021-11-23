using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Character
{
    /// <summary>
    /// Calculates distance and helps to move ANY characters.
    /// </summary>
    public class DistanceCalculator
    {
        protected readonly ICharacter _character;
        protected readonly GameObject _target;
        protected readonly float _sight;
        public int LayerMask { get; }
        protected readonly Vector2 _direction;

        /// <summary>
        /// Constructor for getting distances from target game object, like Patapon-Enemy, Enemy-Patapon, Patapon-Structure etc.
        /// </summary>
        /// <param name="character">The target character. ("from")</param>
        /// <param name="sight">Maximum sight of the target. This is equivalent to raycast distance.</param>
        /// <param name="layerMask">Masks of layers to detect. ("to") Get this value using <see cref="UnityEngine.LayerMask"/>.</param>
        internal DistanceCalculator(ICharacter character, float sight, int layerMask)
        {
            _character = character;
            _target = (character as MonoBehaviour)?.gameObject;
            _sight = sight;
            _direction = _character.MovingDirection;
            LayerMask = layerMask;
        }
        /// <summary>
        /// <see cref="DistanceCalculator"/> for Patapon (also from left to right).
        /// </summary>
        /// <param name="target">The target game object. ("from")</param>
        internal static DistanceCalculator GetPataponDistanceCalculator(Patapons.Patapon target) =>
            target.IsMeleeUnit ?
            new MeleeDistanceCalculator(target, CharacterEnvironment.Sight, UnityEngine.LayerMask.GetMask("structures", "hazorons", "bosses")) :
            new DistanceCalculator(target, CharacterEnvironment.Sight, UnityEngine.LayerMask.GetMask("structures", "hazorons", "bosses"));
        /// <summary>
        /// <see cref="DistanceCalculator"/> for Hazoron (also from right to left).
        /// </summary>
        /// <param name="target">The target game object. ("from")</param>
        internal static DistanceCalculator GetHazoronDistanceCalculator(Hazorons.Hazoron target) =>
            target.IsMeleeUnit ?
            new MeleeDistanceCalculator(target, CharacterEnvironment.Sight, UnityEngine.LayerMask.GetMask("patapons", "bosses")) :
            new DistanceCalculator(target, CharacterEnvironment.Sight, UnityEngine.LayerMask.GetMask("patapons", "bosses"));
        /// <summary>
        /// <see cref="DistanceCalculator"/> for enemy boss (from right to left). Enemy boss represents boss in normal boss killing mission.
        /// </summary>
        /// <param name="target">The boss, as enemy. ("from")</param>
        internal static DistanceCalculator GetBossDistanceCalculator(Bosses.EnemyBoss target) =>
            new DistanceCalculator(target, CharacterEnvironment.Sight, UnityEngine.LayerMask.GetMask("patapons", "hazorons"));
        /// <summary>
        /// <see cref="DistanceCalculator"/> for summoned boss (from left to right).
        /// </summary>
        /// <param name="target">The summoned boss. ("from")</param>
        internal static DistanceCalculator GetBossDistanceCalculator(Bosses.SummonedBoss target) =>
            new DistanceCalculator(target, CharacterEnvironment.Sight, UnityEngine.LayerMask.GetMask("structures", "hazorons", "bosses"));


        //boxcast data
        private static Vector2 _boxSize = new Vector2(0.001f, CharacterEnvironment.MaxYToScan); //size for boxcasting. NOTE: boxcast doesn't catch from initial box position.
        private static Vector2 _boxcastYOffset = (_boxSize.y / 2) * Vector2.up;
        private static float _boxcastXOffset = _boxSize.x * 0.6f;

        /// <summary>
        /// Shoots RayCast to closest structure or enemy and returns the raycast hit if found.
        /// </summary>
        /// <returns>X position as collider hit, Y position as collided game object position.</returns>
        public Vector2? GetClosest() => GetClosest((Vector2)_target.transform.position + _character.AttackDistance * _direction);

        protected virtual Vector2? GetClosest(Vector2 castPoint)//bidirectional
        {
            var raycast = Physics2D.BoxCast(castPoint + _boxcastXOffset * _direction + _boxcastYOffset, _boxSize, 0, -_direction, _character.AttackDistance, LayerMask);
            var p = ReturnInRange(raycast);
            if (p == null)
            {
                raycast = Physics2D.BoxCast(castPoint - _boxcastXOffset * _direction + _boxcastYOffset, _boxSize, 0, _direction, _sight - _character.AttackDistance, LayerMask);
                p = ReturnInRange(raycast);
                if (p == null) return null;
            }
            var bounds = raycast.collider.bounds;
            return new Vector2(bounds.center.x + bounds.size.x * -_direction.x / 2, bounds.center.y - bounds.size.y / 2);

            float? ReturnInRange(RaycastHit2D hit)
            {
                if (hit.collider == null) return null;
                return hit.point.x;
            }
        }

        public virtual float GetSafeForwardPosition(float input)
        {
            var raycast = Physics2D.BoxCast((Vector2)_target.transform.position - _boxSize.x * _direction + _boxcastYOffset, _boxSize, 0, _direction, _sight, LayerMask);
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

        public IEnumerable<IAttackable> GetAllGroundedTargets()
        {
            var all = Physics2D.BoxCastAll((Vector2)_target.transform.position - _sight * Vector2.right, new Vector2(0.1f, 1), 0, Vector2.right, _sight * 2, LayerMask);
            return all.Select(res => res.collider.GetComponentInParent<IAttackable>()).Where(value => value != null);
        }
        public IEnumerable<Collider2D> GetAllTargetsOnFront()
        {
            var all = Physics2D.BoxCastAll(_target.transform.position, _boxSize, 0, _direction, _sight, LayerMask);
            return all.Select(res => res.collider).Where(value => value?.gameObject != null);
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
