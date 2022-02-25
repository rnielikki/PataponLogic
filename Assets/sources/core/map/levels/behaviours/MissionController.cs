using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class MissionController : MonoBehaviour
    {
        [SerializeField]
        float _timeToWait;
        public void FillMissionCondition() => MissionPoint.Current.FilledMissionCondition = true;
        public void Success(bool fillMissionCondition)
        {
            if (fillMissionCondition) FillMissionCondition();
            MissionPoint.Current.EndMission();
        }
        public void Fail(Transform sender)
        {
            transform.position = sender.position;
            var pataponCamareMover = Camera.main.GetComponent<CameraController.PataponCameraMover>();
            pataponCamareMover.FollowPatapon = false;
            pataponCamareMover.SetTarget(transform);

            Camera.main.GetComponent<CameraController.SafeCameraZoom>().ZoomIn(transform);
            MissionPoint.Current.WaitAndFailMission(_timeToWait);
        }
    }
}
