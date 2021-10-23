using UnityEngine;

namespace Core.Character.Patapon
{
    /// <summary>
    /// Distance calculator for one Patapon. Without this, Patapon won't move when attack or defence etc. Attach same object as Patapon script component. USE WITH ANIMATION EVENT.
    /// </summary>
    /// <note>All values are relative to <see cref="PataponManager"/>Position, with own offset index.</note>
    public class PataponDistance : MonoBehaviour
    {
        /// <summary>
        /// Like Tatepon Ponchaka~Ponpon. This is relative to the root Patapon position manager.
        /// </summary>
        public const int RushAttackDistance = 15;
        public const int DodgeDistance = 15;

        private DistanceCalculator _calculator;
        private Vector2 _defaultPosition;
        private float _movingVelocity; //"movement speed" per second

        private Vector2 _targetPosition;

        //All are moved from this
        private Transform _pataponsManagerTransform;

        private float _pataponOffset;
        private float _pataponGroupOffset;

        private bool _isMoving;
        private bool _isMovingAsOffset;
        private void Awake()
        {
            _calculator = DistanceCalculator.GetPataponDistanceCalculator(gameObject);
            _pataponsManagerTransform = GetComponentInParent<PataponsManager>().transform;
            _defaultPosition = transform.position - _pataponsManagerTransform.position;

            var pon = GetComponent<Patapon>();

            _pataponOffset = pon.Index * PataponEnvironment.PataponDistance;
            _pataponGroupOffset = pon.GroupIndex * PataponEnvironment.PataponDistance;

        }

        /// <summary>
        /// Moves Patapon forward to max default distance, in e.g. Tatepon charge attack.
        /// </summary>
        /// <param name="velocity">How fast it will rush (attack movement speed).</param>
        public void MoveRush(float velocity) => MoveTo(RushAttackDistance, velocity);
        /// <summary>
        /// Moves Patapon when got PONPATA song command.
        /// </summary>
        /// <param name="velocity">How fast will it run (movement speed).</param>
        public void MoveBack(float velocity) => MoveAsOffset(-DodgeDistance, velocity);

        /// <summary>
        /// Brings to first line of Patapons Manager position. For example, Chakachaka song of Tatepon and Kibapon will do this.
        /// </summary>
        /// <param name="velocity"></param>
        public void MoveZero(float velocity) => MoveTo(0, velocity);

        /// <summary>
        /// <see cref="MoveTo"/> for Animation event. Int value is position, float value is velocity.
        /// </summary>
        /// <param name="animationEvent">Animation event that provided from editor.</param>
        public void MoveTo(AnimationEvent animationEvent) =>
            MoveTo(animationEvent.intParameter, animationEvent.floatParameter);

        /// <summary>
        /// Move to specific position (*RELATIVE TO WHOLE PATAPON MANAGER) with certain speed.
        /// </summary>
        /// <param name="positionOffset">Position, relative from Patapon Manager 0 position. + is forward, - is backward.</param>
        /// <param name="velocity">Speed, how much will move per second. ALWAYS +.</param>
        public void MoveTo(float positionOffset, float velocity)
        {
            if (velocity <= 0)
            {
                throw new System.ArgumentException("Velocity cannot be 0 or less.");
            }

            _targetPosition = (Vector2)_pataponsManagerTransform.position + (positionOffset + _pataponGroupOffset) * Vector2.right;
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
        public void MoveToInitialPlace()
        {
            if (transform.localPosition.x == _defaultPosition.x)
            {
                _isMoving = false;
            }
            else
            {
                MoveAsOffset(0, 4);
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
                if (_isMovingAsOffset)
                {
                    transform.position = Vector2.MoveTowards(transform.position, _targetPosition + (Vector2)_pataponsManagerTransform.position, step);
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, _targetPosition, step);
                }
                //Don't go over enemy
                var point = _calculator.GetClosest().point;
                if (transform.position.x > point.x)
                {
                    transform.position = new Vector2(point.x - 0.5f, transform.position.y);
                    _isMoving = false;
                }
            }
        }
    }
}
