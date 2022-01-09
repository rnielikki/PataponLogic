using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.Intro
{
    class AlmightyNameInput : MonoBehaviour
    {
        [SerializeField]
        TMPro.TMP_InputField _inputField;
        [SerializeField]
        Button _okButton;
        InputAction _navAction;
        private void Awake()
        {
            _navAction = Core.Global.GlobalData.Input.actions.FindAction("UI/Navigate");
        }
        private void Start()
        {
            _inputField.Select();
            _inputField.ActivateInputField();
        }
        private void OnEnable()
        {
            _inputField.enabled = true;
            _okButton.enabled = true;
            _inputField.Select();
            _inputField.ActivateInputField();
            _navAction.performed += EndWriting;
        }
        private void OnDisable()
        {
            _inputField.DeactivateInputField();
            _inputField.enabled = false;
            _okButton.enabled = false;
            _navAction.performed -= EndWriting;
        }
        public void CheckAlmightyName()
        {
            string name = _inputField.text;
            if (string.IsNullOrWhiteSpace(name))
            {
                Common.GameDisplay.ConfirmDialog.Create("O Great Almighty! Please tell us your name!")
                    .SetTargetToResume(this)
                    .HideOkButton()
                    .SelectCancel();
                Core.Global.GlobalData.Sound.PlayBeep();
            }
            else if (Core.Global.Slots.SlotMetaList.HasName(name))
            {
                Common.GameDisplay.ConfirmDialog.Create($"There's another save that uses same Almighty name:\n{name}\nPlease select another name.")
                    .SetTargetToResume(this)
                    .HideOkButton()
                    .SelectCancel();
                Core.Global.GlobalData.Sound.PlayBeep();
            }
            else
            {
                Common.GameDisplay.ConfirmDialog.Create($"{name}\nIs this the right name?")
                    .SetTargetToResume(this)
                    .SetOkAction(() => SetName(name))
                    .SelectOk();
            }
        }
        private void EndWriting(InputAction.CallbackContext context)
        {
            if (!_inputField.isFocused) return;
            if (context.ReadValue<Vector2>().y != 0)
            {
                _inputField.DeactivateInputField();
                _okButton.Select();
            }
        }
        private void SetName(string text)
        {
            Core.Global.GlobalData.CurrentSlot.SetAlmightyName(text);
            Common.GameDisplay.SceneLoadingAction.Create("Patapolis").ChangeScene();
        }
        private void OnDestroy()
        {
            _navAction.performed -= EndWriting;
        }
    }
}
