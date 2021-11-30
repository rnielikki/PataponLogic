using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.Core.CameraController
{
    public class CameraMover : MonoBehaviour
    {
        public GameObject Target { get; set; }
        private Vector3 _pos;

        /// <summary>
        /// Camera smooth moving per second. Does nothing if <see cref="SmoothMoving" /> is <c>false</c>.
        /// </summary>
        [SerializeField]
        private float _cameraMoveSensitivity;

        /// <summary>
        /// Adds speed when it's input move.
        /// </summary>
        [SerializeField]
        private float _inputTurnSpeed;
        /// <summary>
        /// Camera moving range on INPUT.
        /// </summary>
        [SerializeField]
        private float _moveRange;
        private float _inputMoveOffset;
        private InputAction _action;

        /// <summary>
        /// Determines if the camera position is updated. Don't confued with <see cref="SmoothMoving"/>.
        /// </summary>
        private bool _moving = true;

        /// <summary>
        /// Determines if the camera moves smoothly. <c>false</c> causes direct position assigning from the target.
        /// </summary>
        public bool SmoothMoving { get; set; }

        void Awake()
        {
            _pos = transform.position;
            var input = GlobalData.Input.actions;
            _action = input.FindAction("Player/Camera");
            _action.started += SetInputCameraMove;
            _action.canceled += ReleaseInputCameraMove;
            _action.Enable();
        }

        void LateUpdate()
        {
            if (!_moving) return;

            _pos.x = Target.transform.position.x + _inputMoveOffset;

            if (!SmoothMoving)
            {
                transform.position = _pos;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _pos, (_cameraMoveSensitivity + Mathf.Abs(_inputMoveOffset) * _inputTurnSpeed) * Time.deltaTime);
                if (_pos.x == transform.position.x && _inputMoveOffset == 0) SmoothMoving = false;
            }
        }
        /// <summary>
        /// Stops moving completely and put to initial place.
        /// </summary>
        public void StopMoving()
        {
            _inputMoveOffset = 0;
            _pos.x = Target.transform.position.x;
            _moving = false;
        }
        private void SetInputCameraMove(InputAction.CallbackContext context)
        {
            SmoothMoving = true;
            var value = context.ReadValue<float>();
            _inputMoveOffset = value * _moveRange;
        }
        private void ReleaseInputCameraMove(InputAction.CallbackContext context)
        {
            _inputMoveOffset = 0;
        }

        private void OnDestroy()
        {
            _action.started -= SetInputCameraMove;
            _action.canceled -= ReleaseInputCameraMove;
            _action.Disable();
        }
    }
}
