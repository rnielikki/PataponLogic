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

            var bindingPath = _binding.path;
            _deviceName = bindingPath.Substring(bindingPath.IndexOf('<') + 1, bindingPath.IndexOf('>') - 1);

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
            op.Dispose();
            _action.Enable();
            _image.color = _defaultColor;
            UpdateText(name);
        }
        private void Cancel(InputActionRebindingExtensions.RebindingOperation op)
        {
            op.Dispose();
            _image.color = _defaultColor;
        }

        internal void UpdateText(string displayString)
        {
            _text.text = $"{_binding.name} {displayString} ({_deviceName})";
        }
    }
}
