﻿using PataRoad.Core.Character.Equipments.Weapons;
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
        private Dictionary<int, RareponData> _allAvailableRarepons = new Dictionary<int, RareponData>();
        private Dictionary<int, RareponData> _openRarepons = new Dictionary<int, RareponData>();
        private const string _resourcePath = "Characters/Patapons/Rarepons";

        public RareponInfo()
        {
            //_openRareponIndex = new List<int> { 0 }
            _openRareponIndex = new List<int> { 0, 2 };
            Deserialize();
        }

        private void Deserialize()
        {
            var allRarepons = Resources.LoadAll<RareponData>(_resourcePath);
            foreach (var rarepon in allRarepons)
            {
                if (int.TryParse(rarepon.name, out int index))
                {
                    rarepon.Index = index;
                }
                _allAvailableRarepons.Add(index, rarepon);
            }

            foreach (var index in _openRareponIndex)
            {
                _openRarepons.Add(index, _allAvailableRarepons[index]);
            }
        }
        public RareponData OpenNewRarepon(int index)
        {
            if (!_openRareponIndex.Contains(index))
            {
                _openRareponIndex.Add(index);
                var data = _allAvailableRarepons[index];
                _openRarepons.Add(index, data);
                return data;
            }
            return null;
        }
        public bool HasRarepon(int index) => _openRarepons.ContainsKey(index);
        public RareponData GetFromOpenRarepon(int index)
        {
            if (_openRarepons.TryGetValue(index, out RareponData data)) return data;
            else return null;
        }
        public RareponData LoadResourceWithoutOpen(int index)
        {
            if (!_allAvailableRarepons.TryGetValue(index, out RareponData data)) return null;
            else return data;
        }

        public void OnBeforeSerialize()
        {
            _openRareponIndex = _openRarepons.Keys.ToList();
        }

        public void OnAfterDeserialize()
        {
            Deserialize();
        }
    }
}
