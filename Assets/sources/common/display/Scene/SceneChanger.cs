
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PataRoad.Common
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField]
        string _sceneName;
        public void LoadScene()
        {
            SceneManager.LoadScene(_sceneName);
        }
        public void LoadSceneSmoothly()
        {
            GameDisplay.SceneLoadingAction.Create(_sceneName).ChangeScene(Color.black);
        }
    }
}
