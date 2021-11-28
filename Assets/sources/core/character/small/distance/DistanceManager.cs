using UnityEngine;

namespace PataRoad.Core.Character
{
    /// <summary>
    /// Distance manager for one Patapon. Without this, Patapon won't move when attack or defence etc. Attach same object as Patapon script component. USE WITH ANIMATION EVENT.
    /// </summary>
    /// <remarks>This is NON-CONTINUOS MOVE in turn. When it arrives, it ends moving in same turn. for continuos move, see <see cref="AttackMoveController"/>.</remarks>
    /// <note>All values are relative to <see cref="PataponManager"/>Position, with own offset index.</note>
    public class DistanceManager : MonoBehaviour
    {
        internal DistanceCalculator DistanceCalculator { get; set; }

        protected Vector2 _defaultPosition; //relative to PATAPON GROUP
        public virtual float DefaultWorldPosition { get; protected set; }
        protected float _movingVelocity; //"movement speed" per second

        protected Vector2 _targetPosition;
        protected virtual Vector2 _parentPosition { get; set; } = Vector2.zero;
        protected int _movingDirection;

        public float Offset { get; protected set; }
        public float AttackDistanceWithOffset => _smallCharacter.AttackDistance + Offset * _movingDirection;

        /// <summary>
        /// Returns current x value of <see cref="PataponsManager"/>.
        /// </summary>
        protected SmallCharacter _smallCharacter;

        protected bool _isMoving;
        protected bool _isMovingAsOffset;
        protected bool _ignoreSafeDistance;

        private void Start()
        {
            Init();
            DistanceCalculator = _smallCharacter.DistanceCalculator;
        }
        protected void Init()
        {
            _smallCharacter = GetComponent<SmallCharacter>();
            _movingDirection = (int)_smallCharacter.MovingDirection.x;
            _defaultPosition = transform.position;
            DefaultWorldPosition = _defaultPosition.x;
        }

        /// <summary>
        /// Moves Patapon when got PONPATA song command.
        /// </summary>
        /// <param name="velocity">How fast will it run (movement speed).</param>
        public void MoveBack(float velocity) => MoveAsOffset(-CharacterEnvironment.DodgeDistance * _movingDirection, velocity);

        /// <summary>
        /// Brings to first line of Patapons Manager position. For example, Chakachaka song of Tatepon and Kibapon will do this.
        /// </summary>
        /// <param name="velocity">Speed, how much will move per second. ALWAYS +.</param>
        public void MoveZero(float velocity) => MoveTo(0, velocity);

        /// <summary>
        /// Move to specific position (*RELATIVE TO WHOLE PATAPON MANAGER) with certain speed.
        /// </summary>
        /// <param name="positionOffset">Position, relative from Patapon Manager 0 position. + is forward, - is backward.</param>
        /// <param name="velocity">Speed, how much will move per second. ALWAYS +.</param>
        public virtual void MoveTo(float positionOffset, float velocity, bool ignoreSafeDistance = false)
        {
            float x = DefaultWorldPosition + positionOffset * _movingDirection;
            var hit = DistanceCalculator.GetClosest();
            if (hit != null && !ignoreSafeDistance)
            {
                x = Mathf.Max(
                    hit.Value.x,
                    DefaultWorldPosition
                );
            }
            MoveWithTargetPosition(x, velocity, ignoreSafeDistance);
        }
        /// <summary>
        /// Move to ABSOLUTE specific position with certain speed.
        /// </summary>
        /// <param name="targetX">The global target positoin to move.</param>
        /// <param name="velocity">Speed, how much will move per second. ALWAYS +.</param>
        protected void MoveWithTargetPosition(float targetX, float velocity, bool ignoreSafeDistance = false)
        {
            if (velocity <= 0)
            {
                throw new System.ArgumentException("Velocity cannot be 0 or less.");
            }
            else if (DistanceCalculator.IsInTargetRange(targetX, velocity * Time.deltaTime))
            {
                _isMoving = false;
                return;
            }
            _targetPosition = new Vector2(targetX - Offset * _movingDirection, 0);
            _movingVelocity = velocity;
            _isMovingAsOffset = false;
            _isMoving = true;
            _ignoreSafeDistance = ignoreSafeDistance;
        }

        /// <summary>
        /// Stop moving and do animation in current place.
        /// </summary>
        public void StopMoving()
        {
            _isMoving = false;
        }
        /// <summary>
        /// Move to initial position, for DONCHAKA song etc.
        /// </summary>
        /// <param name="velocity">Speed, how much will move per second. ALWAYS +.</param>
        public void MoveToInitialPlace(float velocity)
        {
            if (DistanceCalculator.IsInTargetRange(transform.localPosition.x, _defaultPosition.x, velocity * Time.deltaTime))
            {
                _isMoving = false;
            }
            else
            {
                MoveAsOffset(0, velocity);
            }
        }

        /// <summary>
        /// Moves relative to Patapon default position.
        /// </summary>
        /// <param name="offset">The offset from the default position.</param>
        /// <param name="velocity">Speed, how much will move per second.</param>
        protected void MoveAsOffset(float offset, float velocity)
        {
            _targetPosition = _defaultPosition + offset * Vector2.right * _movingDirection;
            _movingVelocity = velocity;
            _isMovingAsOffset = true;
            _isMoving = true;
            _ignoreSafeDistance = false;
        }
        protected void Update()
        {
            if (_isMoving)
            {
                if (_smallCharacter.StatusEffectManager.OnStatusEffect)
                {
                    _isMoving = false;
                    return;
                }
                var step = _movingVelocity * Time.deltaTime;
                Vector2 target;
                if (_isMovingAsOffset)
                {
                    target = _targetPosition + _parentPosition;
                }
                else
                {
                    target = _targetPosition;
                }
                if (!_ignoreSafeDistance) target.x = DistanceCalculator.GetSafeForwardPosition(target.x);
                transform.position = Vector2.MoveTowards(transform.position, target, step);
                _isMoving = !DistanceCalculator.IsInTargetRange(target.x, step);
            }
            else
            {
                transform.position = new Vector2(DistanceCalculator.GetSafeForwardPosition(transform.position.x), 0);
            }
        }
    }
}
