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

        //smooth zooming
        [SerializeField]
        float _zoomSpeed = 2;
        private bool _zooming;
        private int _direction;
        private Vector3 _zoomInPosition;
        private float _lerpTime;

        // Start is called before the first frame update
        void Start()
        {
            _lerpTime = 1;
            _camera = GetComponent<Camera>();
            _defaultPosition = transform.position;
            _positionOffset = new Vector3(-4.5f, 2.8f, _defaultPosition.z);
            _zoomOutCameraSize = _camera.orthographicSize;
        }
        public void ZoomIn(Transform target)
        {
            _zoomInPosition = target.position + _positionOffset;
            _zooming = true;
            _direction = -1;
        }
        public void ZoomOut()
        {
            _zooming = true;
            _direction = 1;
        }
        private void Update()
        {
            if (_zooming)
            {
                var offset = _zoomSpeed * Time.deltaTime;
                _lerpTime = Mathf.Clamp01(_lerpTime + offset * _direction);
                transform.position = Vector3.Lerp(_zoomInPosition, _defaultPosition, _lerpTime);
                _camera.orthographicSize = Mathf.Lerp(_zoomInCameraSize, _zoomOutCameraSize, _lerpTime);
                //because lerp result is weird...?
                if ((_lerpTime == 0 && _direction == -1) || (_lerpTime == 1 && _direction == 1))
                {
                    _zooming = false;
                }
            }
        }
    }
}
