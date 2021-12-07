using PataRoad.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace PataRoad.Common
{
    /// <summary>
    /// Loads scene. Useful for showing tip while loading.
    /// </summary>
    public class SceneLoadingAction : MonoBehaviour
    {
        private string _sceneName { get; set; }
        private InputAction _action;
        private bool _useTip;
        private UnityEngine.Events.UnityAction _additionalAction;
        private static System.Collections.Generic.List<SceneLoadingAction> _sceneLoadingActions = new System.Collections.Generic.List<SceneLoadingAction>();
        private void Start()
        {
            _sceneLoadingActions.Add(this);
        }

        /// <summary>
        /// Loads scene when pressed the UI Button.
        /// </summary>
        /// <param name="sceneName">Name of the scene. Do not enter 'Tips' here, use <paramref name="useTip"/> instead.</param>
        /// <param name="useTip">Show tip before loading.</param>
        /// <param name="uiButton">Tne name of button IN UI group.</param>
        public static void Create(string sceneName, bool useTip, string uiButton, UnityEngine.Events.UnityAction additionalAction = null)
        {
            var obj = Instantiate(new GameObject());
            obj.AddComponent<SceneLoadingAction>().Set(sceneName, useTip, uiButton, additionalAction);
        }

        /// <summary>
        /// Loads scene "now" without pressing button.
        /// </summary>
        /// <param name="sceneName">Name of the scene. Do not enter 'Tips' here, use <paramref name="useTip"/> instead.</param>
        /// <param name="useTip">Show tip before loading.</param>
        public static void Create(string sceneName, bool useTip)
        {
            var obj = Instantiate(new GameObject());
            obj.AddComponent<SceneLoadingAction>().Set(sceneName, useTip);
        }
        private void Set(string sceneName, bool useTip, string uiButton, UnityEngine.Events.UnityAction additionalAction = null)
        {
            _action = GlobalData.Input.actions.FindAction("UI/" + uiButton);
            _action.Enable();
            _sceneName = sceneName;
            _useTip = useTip;
            _action.started += ChangeScene;
            _additionalAction = additionalAction;
        }
        private void Set(string sceneName, bool useTip)
        {
            _sceneName = sceneName;
            _useTip = useTip;
            ChangeScene();
        }
        private void ChangeScene(InputAction.CallbackContext context)
        {
            _action.started -= ChangeScene;
            _additionalAction?.Invoke();
            ChangeScene();
        }
        public void ChangeScene()
        {
            foreach (var action in _sceneLoadingActions)
            {
                if (action != this) Destroy(action.gameObject);
            }
            _sceneLoadingActions.Clear();
            GameDisplay.ScreenFading.Create(false, 2, () =>
            {
                if (_useTip)
                {
                    DontDestroyOnLoad(gameObject);
                    SceneManager.LoadScene("Tips");
                    SceneManager.sceneLoaded += SetTipsDisplay;
                }
                else
                {
                    SceneManager.LoadScene(_sceneName);
                }
            });
        }

        private void SetTipsDisplay(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= SetTipsDisplay;
            if (scene.name != "Tips") return;
            FindObjectOfType<GameDisplay.TipDisplay>().LoadNextScene(_sceneName);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= SetTipsDisplay;
            if (_action != null) _action.started -= ChangeScene;
        }
    }
}
