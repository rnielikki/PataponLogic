using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.KeymapSettings
{
    class InputBindingItem : MonoBehaviour, Common.GameDisplay.IScrollListElement, IDeselectHandler
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
        [SerializeField]
        private RectTransform _rectTransform;
        private Button _button;

        [Header("Sounds")]
        [SerializeField]
        private AudioClip _selectSound;
        [SerializeField]
        private AudioClip _rebindingSound;
        [SerializeField]
        private AudioClip _removedSound;

        private InputActionLoader _loader;

        private bool _selected;
        private bool _isNewBinding;

        public Selectable Selectable => _button;

        public RectTransform RectTransform => _rectTransform;

        public int Index { get; private set; }

        internal void Init(InputBinding binding, InputAction action, InputActionLoader loader, int index)
        {
            _action = action;
            _binding = binding;
            _loader = loader;
            Index = index;

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
        internal void MoveToUp() => Index--;
        private void Match(InputActionRebindingExtensions.RebindingOperation op)
        {
            if (op.selectedControl.device.name != _deviceName)
            {
                _loader.Instruction
                    .SetText($"You're trying to edit {_deviceName}. Please try on {op.selectedControl.device.name} or add new one.")
                    .Show()
                    .HideAfterTime(2);
                Core.Global.GlobalData.Sound.PlayBeep();
                op.Cancel();
            }
            else if (!_loader.CurrentActionToggle.IsNoDuplication(_loader.ConvertToBindingPath(op.selectedControl.path), out var duplications, _binding.id))
            {
                _loader.Instruction
                    .SetText($"{op.selectedControl.displayName} is already registered for:\n{string.Join(", ", duplications)}")
                    .Show()
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
            Core.Global.GlobalData.Sound.PlayInScene(_rebindingSound);
            _action.Enable();
            _image.color = _defaultColor;
        }
        private void Cancel(InputActionRebindingExtensions.RebindingOperation op)
        {
            op.Dispose();
            _image.color = _defaultColor;
            _action.Enable();
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
                _action.ChangeBindingWithId(_binding.id).Erase();
                Core.Global.GlobalData.GlobalInputActions.KeyMapModel.RemoveBinding(_binding);

                _loader.MarkAsDeleted(this);

                if (wasEnabled) _action.Enable();
                Destroy(gameObject);
            }
            else
            {
                _action.RemoveBindingOverride(_binding);
                UpdateText(_binding.ToDisplayString());

                if (wasEnabled) _action.Enable();
            }
            Core.Global.GlobalData.Sound.PlayInScene(_removedSound);
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

            Core.Global.GlobalData.Sound.PlayInScene(_selectSound);
            _loader.ScrollList.Scroll(this);
        }
    }
}
