using UnityEngine;

namespace Core.Character.Patapon
{
    /// <summary>
    /// Distance calculator for one Patapon. Without this, Patapon won't move when attack or defence etc. Attach same object as Patapon script component. USE WITH ANIMATION EVENT.
    /// </summary>
    internal class PataponDistance : MonoBehaviour
    {
        private DistanceCalculator _calculator;
        private float _defaultPosition; //only x is meaningful
        private float _movingVelocity; //"movement speed" per second

        [SerializeField]
        private float _maxMovingDistance;
        private float _min;
        private float _max;
        private float _currentMin;
        private float _currentMax;

        private bool _isMoving;
        private void Awake()
        {
            _calculator = DistanceCalculator.GetPataponDistanceCalculator(gameObject);
            _defaultPosition = transform.localPosition.x;
            _max = _defaultPosition + _maxMovingDistance;
            _min = _defaultPosition - _maxMovingDistance;
        }
        /// <summary>
        /// Move with certain speed. 
        /// </summary>
        /// <param name="velocity">Speed, how much will move per second. + is forward, - is backward.</param>
        public void Move(float velocity)
        {
            _isMoving = true;
            _movingVelocity = velocity;
            if (velocity > 0)
            {
                _currentMin = _defaultPosition;
                _currentMax = _max;
            }
            else
            {
                _currentMin = _min;
                _currentMax = _defaultPosition;
            }
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
        public void MoveToInitial()
        {
            if (transform.localPosition.x == _defaultPosition)
            {
                _isMoving = false;
            }
            else
            {
                if (transform.localPosition.x < _defaultPosition)
                {
                    _movingVelocity = 4;
                    _currentMin = _min;
                    _currentMax = _defaultPosition;
                }
                else
                {
                    _movingVelocity = -4;
                    _currentMin = _defaultPosition;
                    _currentMax = _max;
                }
                _isMoving = true;
            }
        }
        private void Update()
        {
            if (_isMoving)
            {
                var x = transform.localPosition.x + _movingVelocity * Time.deltaTime;
                var pos = transform.localPosition;

                pos.x = Mathf.Clamp(_currentMin, x, _currentMax);
                transform.localPosition = pos;
            }
        }
    }
}
