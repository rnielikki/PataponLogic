using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.KeymapSettings
{
    class InputBindingItem : MonoBehaviour
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

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(ListenBinding);
            _defaultColor = _image.color;
        }
        internal void Init(InputBinding binding, InputAction action)
        {
            _action = action;
            _binding = binding;

            _deviceName = GetDeviceName(_binding.path);

            UpdateText(binding.ToDisplayString());
        }
        public void ListenBinding()
        {
            _action.Disable();
            _image.color = _colorWhileBinding;

            var rebind = _action.PerformInteractiveRebinding()
            .WithBindingGroup(_binding.groups)
            .WithBindingMask(_binding)
            .WithCancelingThrough("<Keyboard>/escape");

            rebind.Start().OnPotentialMatch(Match).OnComplete(Complete).OnCancel(Cancel);
        }
        private void Match(InputActionRebindingExtensions.RebindingOperation op)
        {
            if (op.selectedControl.device.name != _deviceName)
            {
                op.Cancel();
            }
            else op.Complete();
        }
        private void Complete(InputActionRebindingExtensions.RebindingOperation op)
        {
            var name = op.selectedControl.displayName;
            var overridePath = op.selectedControl.path;
            var binding = op.bindingMask;
            op.Dispose();

            if (binding != null)
            {
                _binding = binding.Value;
                Core.Global.GlobalData.GlobalInputActions.KeyMapModel.AddOverrideBinding(
                    binding.Value, binding.Value.path, overridePath
                    );
                UpdateText(name);
            }
            _image.color = _defaultColor;
        }
        private void Cancel(InputActionRebindingExtensions.RebindingOperation op)
        {
            op.Dispose();
            _image.color = _defaultColor;
        }

        private string GetDeviceName(string path)
        {
            int start = path.IndexOf('<');
            int end = path.IndexOf('>');

            if (start < 0 || end < 0 || start + 2 >= end)
            {
                //unknown device
                return "unknown";
            }
            return path.Substring(start + 1, end - 1);
        }
        internal void UpdateText(string displayString)
        {
            _text.text = $"{_binding.name} {displayString.ToUpper()} ({_deviceName})";
        }
    }
}
