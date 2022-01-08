using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.KeymapSettings
{
    class InputBindingItem : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField]
        Text _text;
        private InputAction _action;
        private InputBinding _binding;
        private string _deviceName;
        private Color _defaultColor;
        [SerializeField]
        private Image _image;
        [SerializeField]
        private Color _colorWhileBinding;
        [SerializeField]
        private Color _newBindingColor;
        private Button _button;

        private InputActionLoader _loader;

        private bool _selected;
        private bool _isNewBinding;


        internal void Init(InputBinding binding, InputAction action, InputActionLoader loader)
        {
            _action = action;
            _binding = binding;
            _loader = loader;

            _deviceName = GetDeviceName(_binding.path);

            _button = GetComponent<Button>();
            if (!string.IsNullOrEmpty(binding.groups))
            {
                _button.onClick.AddListener(ListenBinding);
            }
            else
            {
                _image.color = _newBindingColor;
                _isNewBinding = true;
            }
            _defaultColor = _image.color;

            UpdateText(binding.ToDisplayString());
        }
        public void ListenBinding()
        {
            _action.Disable();
            _image.color = _colorWhileBinding;

            var rebind = _action.PerformInteractiveRebinding()
            .WithCancelingThrough("<Keyboard>/escape");

            if (_binding.isPartOfComposite)
            {
                rebind.WithBindingMask(_binding);
            }
            else
            {
                rebind.WithBindingGroup(_binding.groups);
            }
            rebind.Start().OnPotentialMatch(Match).OnComplete(Complete).OnCancel(Cancel);
        }
        private void Match(InputActionRebindingExtensions.RebindingOperation op)
        {
            if (op.selectedControl.device.name != _deviceName)
            {
                _loader.Instruction
                    .SetText($"You're trying to edit {_deviceName}. Please try on {op.selectedControl.device.name} or add new one.")
                    .HideAfterTime(2);
                Core.Global.GlobalData.Sound.PlayBeep();
                op.Cancel();
            }
            else op.Complete();
        }
        private void Complete(InputActionRebindingExtensions.RebindingOperation op)
        {
            var name = op.selectedControl.displayName;
            op.Dispose();
            UpdateText(name);
            _image.color = _defaultColor;
        }
        private void Cancel(InputActionRebindingExtensions.RebindingOperation op)
        {
            op.Dispose();
            _image.color = _defaultColor;
        }

        private string GetDeviceName(string path)
        {
            if (path == null) return string.Empty;
            int start = path.IndexOf('<');
            int end = path.IndexOf('>');

            if (start < 0 || end < 0 || start + 2 >= end)
            {
                //unknown device
                return "unknown";
            }
            return path.Substring(start + 1, end - 1);
        }
        private void RemoveBinding(InputAction.CallbackContext context)
        {
            if (!_selected) return;
            var wasEnabled = _action.enabled;
            _action.Disable();
            if (_isNewBinding)
            {
                Core.Global.GlobalData.GlobalInputActions.KeyMapModel.RemoveBinding(_binding);
                var nextTarget = _button.navigation.selectOnUp;
                if (nextTarget != null) nextTarget.Select();
                else _loader.Button.Select();

                if (wasEnabled) _action.Enable();
                Destroy(gameObject);
            }
            else
            {
                _action.RemoveBindingOverride(_binding);
                UpdateText(_binding.ToDisplayString());

                if (wasEnabled) _action.Enable();
            }
        }
        internal void UpdateText(string displayString)
        {
            _text.text = $"{_binding.name} {displayString.ToUpper()} ({_deviceName})";
        }
        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
            Core.Global.GlobalData.GlobalInputActions.CancelAction.performed -= RemoveBinding;
        }
        public void OnDeselect(BaseEventData eventData)
        {
            _selected = false;
            Core.Global.GlobalData.GlobalInputActions.CancelAction.performed -= RemoveBinding;
        }
        public void OnSelect(BaseEventData eventData)
        {
            _selected = true;
            Core.Global.GlobalData.GlobalInputActions.CancelAction.performed += RemoveBinding;
        }
    }
}
