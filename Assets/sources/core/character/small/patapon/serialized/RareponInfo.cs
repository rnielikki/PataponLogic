using PataRoad.Core.Character.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Character.Patapons.Data
{
    /// <summary>
    /// Manages all Rarepon data, including open Rarepons and not-yet-opened Rarepons.
    /// </summary>
    [Serializable]
    public class RareponInfo : ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<RareponDataContainer> _openRareponsForSerialization;
        private static Dictionary<int, RareponData[]> _allAvailableRarepons;
        private Dictionary<int, RareponDataContainer> _openRarepons;
        private const string _resourcePath = "Characters/Patapons/Rarepons";

        public RareponDataContainer DefaultRarepon => _openRarepons[0];

        /// <summary>
        /// Initialises self for FIRST SAVE DATA.
        /// </summary>
        /// <returns>Self.</returns>
        internal RareponInfo Init()
        {
            _openRarepons = new Dictionary<int, RareponDataContainer>();
            OpenNewRarepon(0);
            return this;
        }
        /// <summary>
        /// Loads and indexes all Rarepon data. Expected to be called in VERY FIRST INITIALISATION.
        /// </summary>
        internal static void LoadAll()
        {
            _allAvailableRarepons = new Dictionary<int, RareponData[]>();
            foreach (var rarepon in Resources.LoadAll<RareponData>(_resourcePath))
            {
                if (!_allAvailableRarepons.ContainsKey(rarepon.Index))
                {
                    _allAvailableRarepons.Add(rarepon.Index, new RareponData[3]);
                }
                if (IsValidLevel(rarepon.Level))
                {
                    if (_allAvailableRarepons[rarepon.Index][rarepon.Level - 1] != null)
                    {
                        throw new InvalidOperationException($"{rarepon.Index} with {rarepon.Level} already added for "
                            + $"{_allAvailableRarepons[rarepon.Index][rarepon.Level - 1].Name} so cannot assign to {rarepon.Name}");
                    }
                    _allAvailableRarepons[rarepon.Index][rarepon.Level - 1] = rarepon;
                }
                else
                {
                    throw new ArgumentException(
                        $"Rarepon ({rarepon.Name}/{rarepon.Index}) level ({rarepon.Level}) is incorrect. It must be range of 1-3.");
                }
            }
        }
        /// <summary>
        /// Deserialises saved rarepon data to indexed data. Called when the save data is loaded.
        /// </summary>
        private void Deserialize()
        {
            _openRarepons = new Dictionary<int, RareponDataContainer>();
            foreach (var container in _openRareponsForSerialization)
            {
                container.LoadData();
                _openRarepons.Add(container.RareponIndex, container);
            }
        }
        /// <summary>
        /// Opens a new rarepon and adds new <see cref="RareponDataContainer"/>. The default level is automatically set to 1.
        /// </summary>
        /// <param name="index">index of the rarepon.</param>
        /// <returns><see cref="RareponDataContainer"/> of newly opened Rarepon data. <c>null</c> if corresponding data of the index doesn't exist.</returns>
        public RareponDataContainer OpenNewRarepon(int index)
        {
            if (!_openRarepons.ContainsKey(index))
            {
                var container = new RareponDataContainer(index);
                container.LoadData();
                _openRarepons.Add(index, container);

                return container;
            }
            return null;
        }
        /// <summary>
        /// Checks if a Rarepon is open. This checks from ONLY OPEN Rarepons and doesn't check if the index is valid.
        /// </summary>
        /// <param name="index">The Rarepon index to check if it's open.</param>
        /// <returns><c>true</c> if the Rarepon is open, otherwise <c>false</c>.</returns>
        public bool IsRareponOpen(int index) => _openRarepons.ContainsKey(index);
        public RareponDataContainer GetFromOpenRarepon(int index)
        {
            if (_openRarepons.TryGetValue(index, out RareponDataContainer data)) return data;
            else return null;
        }
        /// <summary>
        /// Gets real one Rarepon Data from the index and level.
        /// </summary>
        /// <param name="rareponIndex">Rarepon index.</param>
        /// <param name="level">Level of the Rarepon.</param>
        /// <returns>Data of the Rarepon with corresponding index and level. <c>null</c> if it doesn't exist.</returns>
        internal static RareponData GetRareponData(int rareponIndex, int level)
        {
            if (!IsValidLevel(level)) throw new ArgumentException(
                $"LEVEL {level} (for {rareponIndex}) is incorrect. Level must be range of 1-3.");
            if (_allAvailableRarepons.TryGetValue(rareponIndex, out RareponData[] data))
            {
                return data[level - 1];
            }
            else return null;
        }

        public void OnBeforeSerialize()
        {
            _openRareponsForSerialization = _openRarepons.Values.ToList();
        }

        public void OnAfterDeserialize()
        {
            Deserialize();
        }
        private static bool IsValidLevel(int level) => level >= 1 && level <= 3;
    }
}
