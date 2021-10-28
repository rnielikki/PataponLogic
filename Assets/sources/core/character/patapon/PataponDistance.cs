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
        /// Like Tatepon Ponchaka~Ponpon. This position is relative to the root Patapon position manager.
        /// </summary>
        public const int RushAttackDistance = 15;
        public const int DodgeDistance = 15;

        private DistanceCalculator _distanceCalculator;
        private Vector2 _defaultPosition;
        private float _movingVelocity; //"movement speed" per second

        private Vector2 _targetPosition;

        //All are moved from this
        private Transform _pataponsManagerTransform;

        private float _pataponOffset;

        private bool _isMoving;
        private bool _isMovingAsOffset;

        private float _attackDistance = -1;
        public float AttackDistanceWithOffset => _attackDistance + _pataponOffset;

        private void Start()
        {
            _distanceCalculator = DistanceCalculator.GetPataponDistanceCalculator(gameObject);
            _pataponsManagerTransform = GetComponentInParent<PataponsManager>().transform;
            _defaultPosition = transform.position - _pataponsManagerTransform.position;
            _pataponOffset = GetComponent<Patapon>().IndexInGroup * PataponEnvironment.AttackDistanceBetweenPatapons;
        }
        /// <summary>
        /// Sets default distance.
        /// </summary>
        /// <param name="attackDistance">Patapon attack distance, without considering Patapon size.</param>
        /// <param name="radiusOffset">Offset, as Patapon size. Default is expected to Patapon radius, but also can be from vehicle's head.</param>
        internal void InitDistance(float attackDistance, float radiusOffset)
        {
            if (_attackDistance != -1) return;
            _pataponOffset += radiusOffset;
            _attackDistance = attackDistance;
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
        /// Check if the Patapon has attack target on their sight.
        /// </summary>
        /// <returns><c>true</c> if Patapon finds obstacle (attack) target to Patapon sight, otherwise <c>false</c>.</returns>
        public bool HasAttackTarget() => _distanceCalculator.GetClosest().collider != null;

        /// <summary>
        /// Move (can go forth or back) for defending, for melee and range units. Unlike <see cref="MoveToAttack"/>, it doesn't go over <see cref="PataponsManager"/>.
        /// </summary>
        /// <param name="velocity">Speed, how much will move per second. ALWAYS +.</param>
        /// <returns>Yield value, when moving is done.</returns>
        public System.Collections.IEnumerator MoveToDefend(float velocity)
        {
            var posX = _distanceCalculator.GetClosest().point.x - _attackDistance;
            yield return MoveToDamage(
                Mathf.Min(posX, _pataponsManagerTransform.position.x),
                velocity);
        }

        /// <summary>
        /// Move (can go forth or back) for attacking, for melee and range units. 0 is expected for melee normal attacks.
        /// </summary>
        /// <param name="velocity">Speed, how much will move per second. ALWAYS +.</param>
        /// <returns>Yield value, when moving is done.</returns>
        public System.Collections.IEnumerator MoveToAttack(float velocity)
        {
            var posX = _distanceCalculator.GetClosest().point.x - _attackDistance;
            yield return MoveToDamage(posX, velocity);
        }

        private System.Collections.IEnumerator MoveToDamage(float posX, float velocity)
        {
            MoveWithTargetPosition(posX, velocity);
            yield return new WaitUntil(() => !_isMoving);
        }

        /// <summary>
        /// Move to specific position (*RELATIVE TO WHOLE PATAPON MANAGER) with certain speed.
        /// </summary>
        /// <param name="positionOffset">Position, relative from Patapon Manager 0 position. + is forward, - is backward.</param>
        /// <param name="velocity">Speed, how much will move per second. ALWAYS +.</param>
        public void MoveTo(float positionOffset, float velocity)
        {
            float x = _pataponsManagerTransform.position.x + positionOffset;
            var hit = _distanceCalculator.GetClosest();
            if (hit.collider != null)
            {
                x = Mathf.Min(
                    hit.point.x,
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
            else if (IsInTargetRange(targetX, velocity * Time.deltaTime))
            {
                _isMoving = false;
                return;
            }
            _targetPosition = new Vector2(targetX - _pataponOffset, 0);
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
            if (IsInTargetRange(transform.localPosition.x, _defaultPosition.x, velocity * Time.deltaTime))
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
                _isMoving = !IsInTargetRange(target.x, step);
            }
        }
        //OFFSET MUST BE +
        private bool IsInTargetRange(float targetX, float offset) => IsInTargetRange(transform.position.x, targetX, offset);
        private bool IsInTargetRange(float x, float targetX, float offset) => targetX - offset < x && x < targetX + offset;
    }
}
