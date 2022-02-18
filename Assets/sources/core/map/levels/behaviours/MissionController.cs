using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class MissionController : MonoBehaviour
    {
        [SerializeField]
        float _timeToWait;
        public void Success(bool fillMissionCondition)
        {
            if (fillMissionCondition) MissionPoint.Current.FilledMissionCondition = true;
            MissionPoint.Current.EndMission();
        }
        public void Fail(Transform sender)
        {
            transform.position = sender.transform.position;
            Camera.main.GetComponent<CameraController.SafeCameraZoom>().ZoomIn(transform);
            MissionPoint.Current.WaitAndFailMission(_timeToWait);
        }
    }
}
