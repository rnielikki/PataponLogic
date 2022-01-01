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
        protected readonly IDistanceCalculatable _character;
        protected readonly GameObject _target;
        public int LayerMask { get; }
        protected readonly Vector2 _direction;
        protected readonly float _xDirection;
        private readonly float _size;

        //boxcast data
        protected Vector2 _boxSize; //size for boxcasting. NOTE: boxcast doesn't catch from initial box position.
        protected Vector2 _boxcastYOffset;
        protected float _boxcastXOffset;

        /// <summary>
        /// Constructor for getting distances from target game object, like Patapon-Enemy, Enemy-Patapon, Patapon-Structure etc.
        /// </summary>
        /// <param name="character">The target character. ("from")</param>
        /// <param name="sight">Maximum sight of the target. This is equivalent to raycast distance.</param>
        /// <param name="layerMask">Masks of layers to detect. ("to") Get this value using <see cref="UnityEngine.LayerMask"/>.</param>
        private DistanceCalculator(IDistanceCalculatable character, int layerMask)
        {
            _character = character;
            _size = (character as SmallCharacter)?.CharacterSize ?? 0;
            _target = (character as MonoBehaviour)?.gameObject;
            _direction = _character.MovingDirection;
            _xDirection = _direction.x;
            LayerMask = layerMask;

            _boxSize = new Vector2(0.001f, CharacterEnvironment.MaxYToScan);
            _boxcastYOffset = (_boxSize.y / 2) * Vector2.up;
            _boxcastXOffset = _boxSize.x * 0.6f;
        }
        internal static DistanceCalculator GetPataponManagerDistanceCalculator(Patapons.PataponsManager pataponsManager) =>
            new DistanceCalculator(pataponsManager, UnityEngine.LayerMask.GetMask("structures", "hazorons", "bosses"));
        /// <summary>
        /// <see cref="DistanceCalculator"/> for Patapon (also from left to right).
        /// </summary>
        /// <param name="target">The target game object. ("from")</param>
        internal static DistanceCalculator GetPataponDistanceCalculator(Patapons.Patapon target) =>
            new DistanceCalculator(target, UnityEngine.LayerMask.GetMask("structures", "hazorons", "bosses"));
        /// <summary>
        /// <see cref="DistanceCalculator"/> for Hazoron (also from right to left).
        /// </summary>
        /// <param name="target">The target game object. ("from")</param>
        internal static DistanceCalculator GetHazoronDistanceCalculator(Hazorons.Hazoron target) =>
            new DistanceCalculator(target, UnityEngine.LayerMask.GetMask("patapons", "bosses"));
        /// <summary>
        /// <see cref="DistanceCalculator"/> for enemy boss (from right to left). Enemy boss represents boss in normal boss killing mission.
        /// </summary>
        /// <param name="target">The boss, as enemy. ("from")</param>
        internal static DistanceCalculator GetBossDistanceCalculator(Bosses.EnemyBoss target) =>
            new DistanceCalculator(target, UnityEngine.LayerMask.GetMask("patapons", "hazorons"));
        /// <summary>
        /// <see cref="DistanceCalculator"/> for summoned boss (from left to right).
        /// </summary>
        /// <param name="target">The summoned boss. ("from")</param>
        internal static DistanceCalculator GetBossDistanceCalculator(Bosses.SummonedBoss target) =>
            new DistanceCalculator(target, UnityEngine.LayerMask.GetMask("structures", "hazorons", "bosses"));

        /// <summary>
        /// Shoots Raycast for marching. Results same as melee unit of <see cref="GetClosest()"/>.
        /// </summary>
        /// <returns>X position as collider hit, Y position as collided game object position.</returns>
        public Vector2? GetClosestForMarch() => GetClosest(0);
        /// <summary>
        /// Shoots RayCast to closest structure or enemy and returns the raycast hit if found.
        /// </summary>
        /// <returns>X position as collider hit, Y position as collided game object position.</returns>
        public Vector2? GetClosest() => GetClosest(_character.AttackDistance);
        private Vector2? GetClosest(float attackDistance)
        {
            var closest = GetClosest((Vector2)_target.transform.position + attackDistance * _direction, attackDistance);
            if (closest != null && closest.Value.x * _xDirection > MaxEnemyDistanceInSight(attackDistance) * _xDirection)
            {
                return null;
            }
            else
            {
                return closest;
            }
        }

        protected virtual Vector2? GetClosest(Vector2 castPoint, float attackDistance)//bidirectional
        {
            var raycast = Physics2D.BoxCast(castPoint + _boxcastXOffset * _direction + _boxcastYOffset, _boxSize, 0, -_direction, attackDistance, LayerMask);
            var p = ReturnInRange(raycast);
            if (p == null)
            {
                raycast = Physics2D.BoxCast(castPoint - _boxcastXOffset * _direction + _boxcastYOffset, _boxSize, 0, _direction, CharacterEnvironment.Sight - attackDistance, LayerMask);
                p = ReturnInRange(raycast);
                if (p == null) return null;
            }
            var bounds = raycast.collider.bounds;
            return new Vector2(bounds.center.x + bounds.size.x * -_xDirection / 2, bounds.center.y - bounds.size.y / 2);

            float? ReturnInRange(RaycastHit2D hit)
            {
                if (hit.collider == null) return null;
                return hit.point.x;
            }
        }

        public virtual float GetSafeForwardPosition(float input)
        {
            var raycast = Physics2D.BoxCast((Vector2)_target.transform.position - _boxSize.x * _direction + _boxcastYOffset, _boxSize, 0, _direction, CharacterEnvironment.Sight, LayerMask);

            if (raycast.collider == null)
            {
                return input;
            }
            if (_xDirection < 0)
            {
                return Mathf.Max(raycast.transform.position.x + (raycast.collider.bounds.size.x * 0.5f) + _size, input);
            }
            else
            {
                return Mathf.Min(raycast.transform.position.x - (raycast.collider.bounds.size.x * 0.5f) - _size, input);
            }
        }

        public IEnumerable<IAttackable> GetAllGroundedTargets()
        {
            var all = Physics2D.BoxCastAll(
                (Vector2)_target.transform.position - CharacterEnvironment.OriginalSight * Vector2.right,
                new Vector2(0.1f, 1), 0, Vector2.right,
                CharacterEnvironment.OriginalSight * 2, LayerMask);
            return all.Select(res => res.collider.GetComponentInParent<IAttackable>()).Where(value => value != null);
        }
        public IEnumerable<Collider2D> GetAllTargetsOnFront()
        {
            var all = Physics2D.BoxCastAll(_target.transform.position, _boxSize, 0, _direction, CharacterEnvironment.OriginalSight, LayerMask);
            return all.Select(res => res.collider).Where(value => value?.gameObject != null);
        }

        public bool IsInTargetRange(float targetX, float offset) => IsInTargetRange(_target.transform.position.x, targetX, offset);
        public bool IsInTargetRange(float x, float targetX, float offset) => targetX - offset < x && x < targetX + offset;
        /// <summary>
        /// Check if the character has attack target on their sight.
        /// </summary>
        /// <returns><c>true</c> if Patapon finds obstacle (attack) target to Patapon sight, otherwise <c>false</c>.</returns>
        public bool HasAttackTarget() => GetClosest() != null;
        protected float MaxEnemyDistanceInSight(float attackDistance) => _character.DefaultWorldPosition + _xDirection * (CharacterEnvironment.Sight + attackDistance);
    }
}
