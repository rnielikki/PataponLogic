using PataRoad.Core;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace PataRoad.Common
{
    class SceneLoaderOnAction
    {
        private readonly string _sceneName;
        private readonly InputAction _action;
        public SceneLoaderOnAction(string sceneName, string uiButton = "Submit")
        {
            _action = GlobalData.Input.actions.FindAction("UI/" + uiButton);
            _sceneName = sceneName;
            _action.performed += ChangeScene;
        }
        private void ChangeScene(InputAction.CallbackContext context) => SceneManager.LoadScene(_sceneName);

        public void Destroy()
        {
            _action.performed -= ChangeScene;
        }
    }
}
