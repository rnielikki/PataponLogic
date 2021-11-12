using PataRoad.Core.Character.Equipments;
using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Items
{
    /// <summary>
    /// Manages equipment loading.
    /// </summary>
    /// <remarks>Inventory can be done with similar logic.</remarks>
    public static class EquipmentDataLoader
    {
        private static readonly Dictionary<string, Dictionary<int, EquipmentData>> _loadedData = new Dictionary<string, Dictionary<int, EquipmentData>>();
        /// <summary>
        /// Loads equpment data and also saves data if not loaded.
        /// </summary>
        /// <param name="name">Name of the data.</param>
        /// <param name="index">Unique index for the data.</param>
        /// <returns>Equipment data.</returns>
        public static EquipmentData GetEquipment(string name, int index)
        {
            if (!_loadedData.TryGetValue(name, out Dictionary<int, EquipmentData> dataSet))
            {
                _loadedData.Add(name, new Dictionary<int, EquipmentData>());
            }
            else if (dataSet.TryGetValue(index, out EquipmentData data))
            {
                return data;
            }

            var loadedResource = Resources.Load<EquipmentData>($"Items/Equipments/{name}/{index}");
            if (loadedResource == null) return null;

            _loadedData[name].Add(index, loadedResource);
            loadedResource.Id = System.Guid.NewGuid();
            return loadedResource;
        }
    }
}
