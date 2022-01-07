using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.Core.Global.Settings
{
    [System.Serializable]
    public class KeymapSettingModel
    {
        [SerializeField]
        List<BindingWrapper> _bindings;
        [SerializeField]
        List<InputBinding> _newBindings;
        internal IEnumerable<BindingWrapper> Bindings => _bindings;
        internal IEnumerable<InputBinding> NewBindings => _newBindings;
        internal KeymapSettingModel()
        {
            _bindings = new List<BindingWrapper>();
            _newBindings = new List<InputBinding>();
        }
        internal void ClearBindings()
        {
            _bindings.Clear();
        }

        internal void AddOverrideBinding(InputBinding binding, string path, string overridePath)
        {
            _bindings.Add(new BindingWrapper(binding, path, overridePath));
        }
        internal void AddNewBinding(InputBinding binding)
        {
            _newBindings.Add(binding);
        }
        public bool DoPathExist(string path)
        {
            foreach (var binding in _bindings)
            {
                if (binding.OverridePath == path) return true;
            }
            foreach (var binding in _newBindings)
            {
                if (binding.effectivePath == path) return true;
            }
            return false;
        }
    }
}
