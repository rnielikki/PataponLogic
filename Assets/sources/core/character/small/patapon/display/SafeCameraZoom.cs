using UnityEngine;

namespace PataRoad.Core.CameraController
{
    /// <summary>
    /// For Moving and Destroyed object scenario.
    /// </summary>
    class SafeCameraZoom : CameraZoom
    {
        [SerializeField]
        CameraMover _cameraMover;
        [SerializeField]
        Character.Patapons.PataponsManager _pataponManager;
        Vector3 _firstPosition;
        protected override Vector3 _defaultPosition => new Vector3(_pataponManager.transform.position.x, _firstPosition.y, _firstPosition.z);
        protected override Vector3 _zoomInPosition => new Vector3((_pataponManager.FirstPatapon?.transform?.position ?? _defaultPosition).x, _firstPosition.y, _firstPosition.z) + _positionOffset;

        private void Start()
        {
            Init();
            _firstPosition = transform.position;
        }
        public override void ZoomIn(Transform target)
        {
            _cameraMover.SetTarget(_pataponManager.FirstPatapon.transform ?? _pataponManager.transform);
            _zooming = true;
            _direction = -1;
        }
        public override void ZoomOut()
        {
            _cameraMover.SetTarget(_pataponManager.transform);
            base.ZoomOut();
        }
        private void Update()
        {
            UpdateZoom();
        }
    }
}
