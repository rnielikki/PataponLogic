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
        /// Moves Patapon when got PONPATA song command.
        /// </summary>
        /// <param name="velocity">How fast will it run (movement speed).</param>
        public void MoveBack(float velocity) => MoveAsOffset(-DodgeDistance, velocity);

        /// <summary>
        /// Moves Patapon forward to max default distance, in e.g. Tatepon charge attack.
        /// </summary>
        /// <param name="velocity">How fast it will rush (attack movement speed).</param>
        public void MoveRush(float velocity) => MoveTo(RushAttackDistance, velocity);

        /// <summary>
        /// Brings to first line of Patapons Manager position. For example, Chakachaka song of Tatepon and Kibapon will do this.
        /// </summary>
        /// <param name="velocity">Speed, how much will move per second. ALWAYS +.</param>
        public void MoveZero(float velocity) => MoveTo(0, velocity);

        /// <summary>
        /// Move (can go forth or back) for attacking, for melee and range units. 0 is expected for melee normal attacks.
        /// </summary>
        /// <param name="AttackDistance">Distance from the target to attack.</param>
        /// <param name="velocity">Speed, how much will move per second. ALWAYS +.</param>
        /// <returns>Yield value, when moving is done.</returns>
        public System.Collections.IEnumerator MoveToAttack(float AttackDistance, float velocity)
        {
            var posX = _calculator.GetClosest().point.x - AttackDistance;
            MoveWithTargetPosition(posX, velocity);
            yield return new WaitUntil(() => transform.position.x == posX);
        }

        /// <summary>
        /// Move to specific position (*RELATIVE TO WHOLE PATAPON MANAGER) with certain speed.
        /// </summary>
        /// <param name="positionOffset">Position, relative from Patapon Manager 0 position. + is forward, - is backward.</param>
        /// <param name="velocity">Speed, how much will move per second. ALWAYS +.</param>
        public void MoveTo(float positionOffset, float velocity)
        {
            var x = Mathf.Min(
                _calculator.GetClosest().point.x,
                _pataponsManagerTransform.position.x + positionOffset + _pataponGroupOffset
                );
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
            _targetPosition = new Vector2(targetX, 0);
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
            }
        }
    }
}
