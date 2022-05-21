using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PataRoad.Common.GameDisplay
{
    public class Pause : MonoBehaviour
    {
        private void Awake()
        {
            Core.Rhythm.RhythmTimer.PauseApplication();
            Core.Global.GlobalData.Input.actions.FindAction("UI/Submit").Enable();
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
            var resumeButton = transform.Find("Resume").GetComponent<Button>();
            resumeButton.onClick.AddListener(Destroy);
            transform.Find("End").GetComponent<Button>().onClick.AddListener(EndMission);
            resumeButton.Select();
            resumeButton.OnSelect(null);
        }
        private void OnDestroy()
        {
            Core.Rhythm.RhythmTimer.ResumeApplication();
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
        }
        private void Destroy() => Destroy(gameObject);
        private void EndMission()
        {
            Core.Map.MissionPoint.Current
            .FailMissionNow();
            Destroy();
        }
    }
}
