using UnityEngine;

namespace PataRoad.Common
{
    public class CameraFollower : MonoBehaviour
    {
        Transform _camera;
        // Start is called before the first frame update
        void Awake()
        {
            _camera = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            var t = transform.position;
            t.x = _camera.position.x;
            transform.position = t;
        }
    }
}
