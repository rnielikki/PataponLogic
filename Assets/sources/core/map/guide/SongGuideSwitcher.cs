using PataRoad.Common.GameDisplay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.Core.Map
{
    class SongGuideSwitcher : MonoBehaviour
    {
        [SerializeField]
        string _actionName;
        private InputAction _action;
        [SerializeField]
        GuideDisplayManager _guideDisplayManager;
        [SerializeField]
        GuideDisplay _commandDisplay;
        [SerializeField]
        GuideDisplay _nonCommandDisplay;
        private void Start()
        {
            var input = Global.GlobalData.Input.actions;
            _action = input.FindAction(_actionName);
            _action.started += ShowCommands;
            _action.canceled += HideCommands;
        }
        private void ShowCommands(InputAction.CallbackContext context)
        {
            _guideDisplayManager.ChangeDisplay(_commandDisplay);
        }
        private void HideCommands(InputAction.CallbackContext context)
        {
            _guideDisplayManager.ChangeDisplay(_nonCommandDisplay);
        }
        private void OnDestroy()
        {
            _action.started -= ShowCommands;
            _action.canceled -= HideCommands;
            _action.Disable();
        }

    }
}
