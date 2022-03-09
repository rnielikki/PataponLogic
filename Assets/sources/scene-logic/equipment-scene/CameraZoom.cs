using UnityEngine;

namespace PataRoad.Core.CameraController
{
    public class CameraZoom : MonoBehaviour
    {
        protected Camera _camera;
        protected virtual Vector3 _defaultPosition { get; set; }
        protected Vector3 _positionOffset;
        [SerializeField]
        protected float _zoomInCameraSize = 4;
        protected float _zoomOutCameraSize;

        //smooth zooming
        [SerializeField]
        protected float _zoomSpeed = 2;
        [SerializeField]
        protected Vector2 _offset;
        protected bool _zooming;
        protected int _direction;
        protected virtual Vector3 _zoomInPosition { get; set; }
        protected float _lerpTime;

        // Start is called before the first frame update
        void Start()
        {
            Init();
        }
        protected void Init()
        {
            _lerpTime = 1;
            _camera = GetComponent<Camera>();
            _defaultPosition = transform.position;
            _positionOffset = new Vector3(_offset.x, _offset.y, _defaultPosition.z);
            _zoomOutCameraSize = _camera.orthographicSize;
        }
        public virtual void ZoomIn(Transform target)
        {
            _zoomInPosition = target.position + _positionOffset;
            _zooming = true;
            _direction = -1;
        }
        public virtual void ZoomOut()
        {
            _zooming = true;
            _direction = 1;
        }
        protected void UpdateZoom()
        {
            if (_zooming)
            {
                var offset = _zoomSpeed * Time.deltaTime;
                _lerpTime = Mathf.Clamp01(_lerpTime + (offset * _direction));
                transform.position = Vector3.Lerp(_zoomInPosition, _defaultPosition, _lerpTime);
                _camera.orthographicSize = Mathf.Lerp(_zoomInCameraSize, _zoomOutCameraSize, _lerpTime);
                //because lerp result is weird...?
                if ((_lerpTime == 0 && _direction == -1) || (_lerpTime == 1 && _direction == 1))
                {
                    _zooming = false;
                    if (_direction < 0) AfterZoomIn();
                    else AfterZoomOut();
                }
            }
        }
        protected virtual void AfterZoomIn() { }
        protected virtual void AfterZoomOut() { }
        private void Update()
        {
            UpdateZoom();
        }
    }
}
