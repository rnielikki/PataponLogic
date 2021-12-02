using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class CameraZoom : MonoBehaviour
    {
        private Camera _camera;
        Vector3 _defaultPosition;
        Vector3 _positionOffset;
        const float _zoomInCameraSize = 4;
        float _zoomOutCameraSize;
        // Start is called before the first frame update
        void Start()
        {
            _camera = GetComponent<Camera>();
            _defaultPosition = transform.position;
            _positionOffset = new Vector3(-4.5f, 2.8f, _defaultPosition.z);
            _zoomOutCameraSize = _camera.orthographicSize;
        }
        public void ZoomIn(Transform target)
        {
            transform.position = target.position + _positionOffset;
            _camera.orthographicSize = _zoomInCameraSize;
        }
        public void ZoomOut()
        {
            transform.position = _defaultPosition;
            _camera.orthographicSize = _zoomOutCameraSize;
        }
    }
}
