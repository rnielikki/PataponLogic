using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.Core.Global.Settings
{
    [System.Serializable]
    class BindingWrapper
    {
        [SerializeField]
        string _path;
        [SerializeField]
        string _overridePath;
        public string Path => _path;
        [SerializeField]
        InputBinding _binding;
        public InputBinding Binding => _binding;
        internal BindingWrapper(InputBinding binding, string path)
        {
            _path = path;
            _binding = binding;
            _overridePath = _binding.path;
        }
        internal void SetPath()
        {
            _binding.path = _path;
            _binding.overridePath = _overridePath;
        }
    }
}
