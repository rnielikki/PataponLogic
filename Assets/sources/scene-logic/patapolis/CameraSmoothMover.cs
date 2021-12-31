using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis
{
    internal class CameraSmoothMover : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;
        [SerializeField]
        private float _velocity;
        private float _currentVelocity;
        private float _acceleration;
        internal bool IsMoving { get; private set; }
        private Vector3 _targetPosition;

        public void MoveTo(float xPosition)
        {
            var position = _camera.transform.position;
            position.x = xPosition;
            _targetPosition = position;
            var distance = Mathf.Abs(transform.position.x - _targetPosition.x);

            _currentVelocity = _velocity;
            _acceleration = _velocity * _velocity / (distance * 2);
            IsMoving = true;
        }
        private void Update()
        {
            if (IsMoving)
            {
                _currentVelocity -= Time.deltaTime * _acceleration;
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _currentVelocity * Time.deltaTime);
                if (_currentVelocity <= 0 || transform.position.x == _targetPosition.x)
                {
                    transform.position = _targetPosition;
                    IsMoving = false;
                }
            }
        }
    }
}
