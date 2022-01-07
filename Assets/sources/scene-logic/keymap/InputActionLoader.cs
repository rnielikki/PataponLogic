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
        public InputAction CurrentAction { get; private set; }
        private void Start()
        {
            _firstSelect.Select();
            Load("Drum/Pata");
        }
        public void Load(string inputAction)
        {
            foreach (var child in _attachTarget.GetComponentsInChildren<InputBindingItem>())
            {
                Destroy(child.gameObject);
            }
            var action = GlobalData.Input.actions.FindAction(inputAction);
            CurrentAction = action;
            foreach (var binding in action.bindings)
            {
                if (!binding.isComposite)
                {
                    Instantiate(_template, _attachTarget).Init(binding, action);
                }
            }
        }
        internal void Attach(InputBinding newBinding)
        {
            Instantiate(_template, _attachTarget).Init(newBinding, CurrentAction);
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
