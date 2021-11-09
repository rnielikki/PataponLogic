using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PataRoad.GameDisplay
{
    public class Pause : MonoBehaviour
    {
        private void Awake()
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
            transform.Find("Resume").GetComponent<Button>().onClick.AddListener(Destroy);
            transform.Find("End").GetComponent<Button>().onClick.AddListener(EndMission);
        }
        private void OnDestroy()
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
        }
        private void Destroy() => Destroy(gameObject);
        private void EndMission()
        {
            GameObject.FindGameObjectWithTag("Finish")
            .GetComponent<Core.Map.MissionPoint>()
            .FailMissionNow();
            Destroy();
        }
    }
}
