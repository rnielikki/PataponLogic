using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis
{
    internal class CameraSmoothMover : MonoBehaviour
    {
        [SerializeField]
        private float _sensitivity;
        [SerializeField]
        private Camera _camera;
        internal bool IsMoving { get; private set; }
        private Vector3 _targetPosition;
        public void MoveTo(float xPosition)
        {
            var position = _camera.transform.position;
            position.x = xPosition;
            _targetPosition = position;
            IsMoving = true;
        }
        private void Update()
        {
            if (IsMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _sensitivity * Time.deltaTime);
                if (transform.position == _targetPosition) IsMoving = false;
            }
        }
    }
}
