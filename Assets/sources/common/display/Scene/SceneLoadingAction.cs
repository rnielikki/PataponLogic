using UnityEngine;
using UnityEngine.SceneManagement;

namespace PataRoad.Common.GameDisplay
{
    /// <summary>
    /// Loads scene. Useful for showing tip while loading.
    /// </summary>
    public class SceneLoadingAction : MonoBehaviour
    {
        private string _sceneName { get; set; }
        private bool _useTip;

        public static SceneLoadingAction Create(string sceneName)
        {
            var obj = Instantiate(new GameObject());
            var action = obj.AddComponent<SceneLoadingAction>();
            action._sceneName = sceneName;
            return action;
        }

        public SceneLoadingAction UseTip()
        {
            _useTip = true;
            return this;
        }

        public void ChangeScene()
        {
            DontDestroyOnLoad(gameObject);
            ScreenFading.Create(false, 3, () =>
            {
                if (_useTip)
                {
                    SceneManager.LoadScene("Tips");
                    SceneManager.sceneLoaded += SetTipsDisplay;
                }
                else
                {
                    ScreenFading.Create(true, 2);
                    SceneManager.LoadScene(_sceneName);
                }
                SceneManager.sceneLoaded += DestroyThis;
            });
        }

        private void SetTipsDisplay(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= SetTipsDisplay;
            if (scene.name != "Tips") return;
            FindObjectOfType<TipDisplay>().LoadNextScene(_sceneName);
            Destroy(gameObject);
        }
        private void DestroyThis(Scene scene, LoadSceneMode mode)
        {
            Destroy(gameObject);
            SceneManager.sceneLoaded -= DestroyThis;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= SetTipsDisplay;
            SceneManager.sceneLoaded -= DestroyThis;
        }
    }
}
