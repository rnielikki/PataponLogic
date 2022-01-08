using PataRoad.Core.Global;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.SceneLogic.KeymapSettings
{
    class InputActionLoader : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Selectable _firstSelect;
        [SerializeField]
        private InputBindingItem _template;
        [SerializeField]
        private RectTransform _attachTarget;
        [SerializeField]
        InputBindingAdder _adder;
        private InputAction _currentAction;

        [SerializeField]
        private UnityEngine.UI.Button _okButton;
        public UnityEngine.UI.Button Button => _okButton;
        [SerializeField]
        private UnityEngine.UI.Button _resetButton;

        [SerializeField]
        private InstructionWindow _instruction;
        public InstructionWindow Instruction => _instruction;

        private void Start()
        {
            GetComponentInChildren<UnityEngine.UI.Toggle>().isOn = true;
            _firstSelect.Select();
            _adder.Init(this);
        }
        public void Load(InputAction action, ActionToggleItem currentItem)
        {
            _currentAction = action;
            _adder.SetListeningType(action, currentItem);
            Refresh();
        }
        internal void Refresh()
        {
            foreach (var child in _attachTarget.GetComponentsInChildren<InputBindingItem>())
            {
                Destroy(child.gameObject);
            }
            foreach (var binding in _currentAction.bindings)
            {
                if (!binding.isComposite && binding.path != null)
                {
                    Instantiate(_template, _attachTarget).Init(binding, _currentAction, this);
                }
            }
        }
        internal void Attach(InputBinding newBinding)
        {
            Instantiate(_template, _attachTarget).Init(newBinding, _currentAction, this);
        }
        public void Save()
        {
            GlobalData.GlobalInputActions.Save();
            ToMain();
        }
        public void Cancel()
        {
            ToMain();
        }
        private void ToMain()
        {
            Common.GameDisplay.SceneLoadingAction.Create("Main").ChangeScene();
        }
        public void ResetBindings()
        {
            Common.GameDisplay.ConfirmDialog.Create("All key binding settings are reset.\nYou can't cancel the action once after it's proceeded.\nAre you sure to proceed?")
                .SetLastSelected(_resetButton.gameObject)
                .SetOkAction(() =>
                {
                    GlobalData.Input.actions.RemoveAllBindingOverrides();
                    GlobalData.GlobalInputActions.KeyMapModel.ClearAllBindings();
                    Refresh();
                })
                .SetTargetToResume(this)
                .SelectCancel();
        }
    }
}
