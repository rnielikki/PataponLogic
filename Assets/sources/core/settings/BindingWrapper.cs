using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.Core.Global.Settings
{
    [System.Serializable]
    class BindingWrapper
    {
        [SerializeField]
        string _path;
        public string Path => _path;
        [SerializeField]
        string _overridePath;
        public string OverridePath => _overridePath;
        [SerializeField]
        InputBinding _binding;
        public InputBinding Binding => _binding;
        internal BindingWrapper(InputBinding binding, string path, string overridePath)
        {
            _path = path;
            _binding = binding;
            _overridePath = overridePath;
        }
        internal void SetPath()
        {
            _binding.path = _path;
            _binding.overridePath = _overridePath;
        }
    }
}
