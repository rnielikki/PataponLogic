using System;
using UnityEngine;

namespace Core.CameraController
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
        /// Determines if the camera position is updated. Don't confued with <see cref="SmoothMoving"/>.
        /// </summary>
        public bool Moving { get; set; } = true;

        /// <summary>
        /// Determines if the camera moves smoothly. <c>false</c> causes direct position assigning from the target.
        /// </summary>
        public bool SmoothMoving { get; set; }

        void Awake()
        {
            _pos = transform.position;
        }

        void Update()
        {
            if (!Moving) return;

            _pos.x = Target.transform.position.x;

            if (!SmoothMoving)
            {
                transform.position = _pos;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _pos, _cameraMoveSensitivity * Time.deltaTime);
                if (_pos.x == transform.position.x) SmoothMoving = false;
            }
        }
    }
}
