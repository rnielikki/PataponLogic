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
        private void Start()
        {
            _firstSelect.Select();
            _adder.Init(this);
            Load("Drum/Pata");
        }
        public void Load(string inputAction)
        {
            var action = GlobalData.Input.actions.FindAction(inputAction);
            _currentAction = action;
            _adder.SetListeningType(action);
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
                if (!binding.isComposite)
                {
                    Instantiate(_template, _attachTarget).Init(binding, _currentAction);
                }
            }
        }
        internal void Attach(InputBinding newBinding)
        {
            Instantiate(_template, _attachTarget).Init(newBinding, _currentAction);
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
    }
}
