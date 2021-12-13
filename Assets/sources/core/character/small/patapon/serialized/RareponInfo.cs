using PataRoad.Core.Character.Equipments.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Character.Patapons.Data
{
    [Serializable]
    public class RareponInfo : ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<int> _openRareponIndex;
        private Dictionary<int, RareponData> _availableRarepons;
        private const string _resourcePath = "Characters/Patapons/Rarepons";
        public RareponInfo()
        {
            //_openRareponIndex = new List<int> { 0 }
            _openRareponIndex = new List<int> { 0, 2 };
            Deserialize();
        }

        private void Deserialize()
        {
            _availableRarepons = new Dictionary<int, RareponData>();
            foreach (var index in _openRareponIndex)
            {
                LoadResource(index);
            }
        }
        public RareponData OpenNewRarepon(int index)
        {
            if (!_openRareponIndex.Contains(index))
            {
                _openRareponIndex.Add(index);
                return LoadResource(index);
            }
            return null;
        }
        public bool HasRarepon(int index) => _availableRarepons.ContainsKey(index);
        public RareponData GetRarepon(int index)
        {
            if (_availableRarepons.TryGetValue(index, out RareponData data)) return data;
            else return null;
        }
        private RareponData LoadResource(int index)
        {
            var res = Resources.Load<RareponData>($"{_resourcePath}/{index}");
            if (!HasRarepon(index) && res != null)
            {
                var instantiated = UnityEngine.Object.Instantiate(res);
                instantiated.Index = index;
                _availableRarepons.Add(index, instantiated);
                return instantiated;
            }
            else return null;
        }
        public void OnBeforeSerialize()
        {
            _openRareponIndex = _availableRarepons.Keys.ToList();
        }

        public void OnAfterDeserialize()
        {
            Deserialize();
        }
    }
}
