using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.KeymapSettings
{
    class ActionToggleItem : MonoBehaviour
    {
        [SerializeField]
        string _actionName;
        [SerializeField]
        string[] _relatedActionNames;
        [SerializeField]
        Toggle _toggle;
        [SerializeField]
        InputActionLoader _loader;
        private InputAction _action;
        private InputAction[] _relatedActions;
        [SerializeField]
        private Image _image;
        [SerializeField]
        private Color _toggleOnColor;
        private Color _toggleOffColor;
        private void Start()
        {
            _toggleOffColor = _image.color;
            _toggle.onValueChanged.AddListener(Load);
            _action = FindAction(_actionName);
            _relatedActions = new InputAction[_relatedActionNames.Length + 1];
            _relatedActions[0] = _action;
            for (int i = 1; i < _relatedActions.Length; i++)
            {
                _relatedActions[i] = FindAction(_relatedActionNames[i - 1]);
            }
        }
        private void Load(bool on)
        {
            if (on)
            {
                _image.color = _toggleOnColor;
                _loader.Load(_action, this);
            }
            else
            {
                _image.color = _toggleOffColor;
            }
        }
        private InputAction FindAction(string actionName) => Core.Global.GlobalData.Input.actions.FindAction(actionName);
        public bool IsNoDuplication(string bindingPath)
        {
            foreach (var relatedAction in _relatedActions)
            {
                foreach (var relatedBinding in relatedAction.bindings)
                {
                    if (relatedBinding.effectivePath == bindingPath)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
