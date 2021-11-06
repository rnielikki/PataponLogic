using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.GameDisplay
{
    class PauseLoader : MonoBehaviour
    {
        private InputAction _action;
        private GameObject _pauseMenu;
        private void Awake()
        {
            var actions = GetComponent<PlayerInput>().actions;
            _action = actions.FindAction("UI/Cancel");
            _action.started += Load;
            _action.Enable();
        }

        private void OnDestroy()
        {
            _action.started -= Load;
            _action.Disable();
        }
        private void Load(InputAction.CallbackContext context)
        {
            if (Core.Map.MissionPoint.IsMissionEnd) return;
            if (_pauseMenu == null)
            {
                _pauseMenu = Instantiate(Resources.Load<GameObject>("Display/Pause"));
            }
            else
            {
                Destroy(_pauseMenu);
                _pauseMenu = null;
            }
        }
    }
}
