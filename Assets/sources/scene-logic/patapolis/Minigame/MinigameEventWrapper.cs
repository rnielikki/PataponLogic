using UnityEngine;
using UnityEngine.SceneManagement;

namespace PataRoad.SceneLogic.Patapolis.Minigame
{
    public class MinigameEventWrapper : MonoBehaviour
    {
        [SerializeField]
        UnityEngine.Events.UnityEvent _onMinigameStarted;
        [SerializeField]
        UnityEngine.Events.UnityEvent _onMinigameEnd;
        private void Start()
        {
            SceneManager.sceneLoaded += InvokeStart;
            SceneManager.sceneUnloaded += InvokeEnd;
        }

        public void InvokeStart(Scene scene, LoadSceneMode mode)
        {
            if (mode != LoadSceneMode.Additive && scene.name != "Minigame") return;
            _onMinigameStarted.Invoke();
        }
        private void InvokeEnd(Scene scene)
        {
            if (scene.name != "Minigame") return;
            _onMinigameEnd.Invoke();
            Core.Global.GlobalData.GlobalInputActions.EnableAllInputs();
        }
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= InvokeStart;
            SceneManager.sceneUnloaded -= InvokeEnd;
        }
    }
}
