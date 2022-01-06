using PataRoad.Core.Global;
using UnityEngine;

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
        private void Start()
        {
            _firstSelect.Select();
            Load("Drum/Pata");
        }
        public void Load(string inputAction)
        {
            foreach (Transform child in _attachTarget.transform)
            {
                Destroy(child.gameObject);
            }
            var action = GlobalData.Input.actions.FindAction(inputAction);
            foreach (var binding in action.bindings)
            {
                if (!binding.isComposite)
                {
                    Instantiate(_template, _attachTarget).Init(binding, action);
                }
            }
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
