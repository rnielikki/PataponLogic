using UnityEngine;

namespace Core.Character.Patapon
{
    /// <summary>
    /// Distance manager for one Patapon. Without this, Patapon won't move when attack or defence etc. Attach same object as Patapon script component. USE WITH ANIMATION EVENT.
    /// </summary>
    /// <remarks>This is NON-CONTINUOS MOVE in turn. When it arrives, it ends moving in same turn. for continuos move, see <see cref="AttackMoveController"/>.</remarks>
    /// <note>All values are relative to <see cref="PataponManager"/>Position, with own offset index.</note>
    public class PataponDistanceManager : MonoBehaviour
    {
        internal DistanceCalculator DistanceCalculator { get; set; }

        private Vector2 _defaultPosition; //relative to PATAPON MANAGER!
        public float DefaultWorldPosition => _defaultPosition.x + _pataponsManagerTransform.position.x;
        private float _movingVelocity; //"movement speed" per second

        private Vector2 _targetPosition;

        //All are moved from this
        private Transform _pataponsManagerTransform;

        /// <summary>
        /// Returns current x value of <see cref="PataponsManager"/>.
        /// </summary>
        public float Front => _pataponsManagerTransform.position.x;

        public float PataponOffset { get; private set; }

        private Patapon _patapon;

        private bool _isMoving;
        private bool _isMovingAsOffset;

        public float AttackDistanceWithOffset => _patapon.AttackDistance + PataponOffset;

        private void Start()
        {
            _patapon = GetComponent<Patapon>();
            _pataponsManagerTransform = GetComponentInParent<PataponsManager>().transform;
            _defaultPosition = transform.position - _pataponsManagerTransform.position;
            PataponOffset = _patapon.IndexInGroup * PataponEnvironment.AttackDistanceBetweenPatapons;
        }

        /// <summary>
        /// Moves Patapon when got PONPATA song command.
        /// </summary>
        /// <param name="velocity">How fast will it run (movement speed).</param>
        public void MoveBack(float velocity) => MoveAsOffset(-PataponEnvironment.DodgeDistance, velocity);

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
        public void MoveTo(float positionOffset, float velocity)
        {
            float x = _pataponsManagerTransform.position.x + positionOffset;
            var hit = DistanceCalculator.GetClosest();
            if (hit != null)
            {
                x = Mathf.Min(
                    hit.Value.x,
                    x
                );
            }
            MoveWithTargetPosition(x, velocity);
        }
        /// <summary>
        /// Move to ABSOLUTE specific position with certain speed.
        /// </summary>
        /// <param name="targetX">The global target positoin to move.</param>
        /// <param name="velocity">Speed, how much will move per second. ALWAYS +.</param>
        private void MoveWithTargetPosition(float targetX, float velocity)
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
            _targetPosition = new Vector2(targetX - PataponOffset, 0);
            _movingVelocity = velocity;
            _isMovingAsOffset = false;
            _isMoving = true;
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
        private void MoveAsOffset(float offset, float velocity)
        {
            _targetPosition = _defaultPosition + offset * Vector2.right;
            _movingVelocity = velocity;
            _isMovingAsOffset = true;
            _isMoving = true;
        }
        private void Update()
        {
            if (_isMoving)
            {
                var step = _movingVelocity * Time.deltaTime;
                Vector2 target;
                if (_isMovingAsOffset)
                {
                    target = _targetPosition + (Vector2)_pataponsManagerTransform.position;
                }
                else
                {
                    target = _targetPosition;
                }

                transform.position = Vector2.MoveTowards(transform.position, target, step);
                _isMoving = !DistanceCalculator.IsInTargetRange(target.x, step);
            }
        }
    }
}
