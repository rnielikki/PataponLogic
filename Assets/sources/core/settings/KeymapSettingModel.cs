using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.Core.Global.Settings
{
    [System.Serializable]
    class KeymapSettingModel : ISerializationCallbackReceiver
    {
        List<BindingWrapper> _bindings;
        [SerializeField]
        BindingWrapper[] _arr;
        internal BindingWrapper[] Bindings => _arr;
        internal KeymapSettingModel()
        {
            _bindings = new List<BindingWrapper>();
        }

        internal void AddToList(InputBinding binding, string path)
        {
            _bindings.Add(new BindingWrapper(binding, path));
        }

        public void OnBeforeSerialize()
        {
            _arr = _bindings.ToArray();
        }

        public void OnAfterDeserialize()
        {
            //whatever
        }
    }
}
