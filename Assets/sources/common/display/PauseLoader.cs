using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.Common.GameDisplay
{
    class PauseLoader : MonoBehaviour
    {
        private InputAction _action;
        private GameObject _pauseMenu;
        [SerializeField]
        private string _actionName;
        private void Awake()
        {
            var actions = Core.Global.GlobalData.Input.actions;
            _action = actions.FindAction(_actionName);
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
                _pauseMenu = Instantiate(Resources.Load<GameObject>("Map/Display/Pause"));
            }
            else
            {
                Destroy(_pauseMenu);
                _pauseMenu = null;
            }
        }
    }
}
