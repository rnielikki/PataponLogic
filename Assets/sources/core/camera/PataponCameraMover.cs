using UnityEngine;

namespace PataRoad.Core.CameraController
{
    class PataponCameraMover : CameraMover
    {
        internal Character.Patapons.PataponsManager Manager { get; set; }
        public override Transform Target => FollowPatapon ? GetTransformWithMaxPosition() : _alternativeTarget;
        private Transform _alternativeTarget;
        public bool FollowPatapon { get; set; } = true;
        public override void SetTarget(Transform targetTransform, bool smooth = true)
        {
            SmoothMoving = smooth;
            _alternativeTarget = targetTransform;
        }
        private Transform GetTransformWithMaxPosition()
        {
            var first = Manager.FirstPatapon;
            if (first == null) return Manager.transform;
            else if (first.transform.position.x > Manager.transform.position.x)
            {
                return first.transform;
            }
            else
            {
                return Manager.transform;
            }
        }
    }
}
