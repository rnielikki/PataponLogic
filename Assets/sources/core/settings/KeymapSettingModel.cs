using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.Core.Global.Settings
{
    /// <summary>
    /// Saves key bindings which cannot be added by <see cref="InputActionAsset.FromJson(string)" />.
    /// </summary>
    [System.Serializable]
    public class KeymapSettingModel
    {
        [SerializeField]
        List<InputBinding> _newBindings;
        internal IEnumerable<InputBinding> NewBindings => _newBindings;
        internal Dictionary<System.Guid, InputActionSetupExtensions.BindingSyntax> _bindingMap;
        internal KeymapSettingModel()
        {
            _newBindings = new List<InputBinding>();
            _bindingMap = new Dictionary<System.Guid, InputActionSetupExtensions.BindingSyntax>();
        }
        internal void AddNewBinding(InputBinding binding)
        {
            if (!_newBindings.Contains(binding)) _newBindings.Add(binding);
        }
        internal void RemoveBinding(InputBinding binding)
        {
            _newBindings.Remove(binding);
            RemoveBindingFromMap(binding);
        }
        private void RemoveBindingFromMap(InputBinding binding)
        {
            if (_bindingMap.ContainsKey(binding.id))
            {
                _bindingMap[binding.id].Erase();
                _bindingMap.Remove(binding.id);
            }
        }
        internal void ClearAllBindings()
        {
            foreach (var binding in _newBindings)
            {
                var action = GlobalData.Input.actions.FindAction(binding.action);

                var wasEnabled = action.enabled;
                action.Disable();

                RemoveBindingFromMap(binding);

                if (wasEnabled) action.Enable();
            }
            _newBindings.Clear();
        }
        internal void LoadBindings(PlayerInput input)
        {
            foreach (var binding in NewBindings)
            {
                var action = input.actions.FindAction(binding.action);
                var bindingSyntax = action.AddBinding(binding);
                _bindingMap.Add(binding.id, bindingSyntax);
            }
        }
    }
}
