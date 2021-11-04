using UnityEngine;

namespace Core.CameraController
{
    public class CameraMover : MonoBehaviour
    {
        public GameObject Target { get; set; }
        private Vector3 _pos;
        public bool Moving { get; set; } = true;

        // Start is called before the first frame update
        void Awake()
        {
            _pos = transform.position;
        }

        void Update()
        {
            if (!Moving) return;
            _pos.x = Target.transform.position.x;
            transform.position = _pos;
        }
    }
}
