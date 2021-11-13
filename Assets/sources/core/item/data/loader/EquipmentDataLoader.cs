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
        public static EquipmentData GetEquipment(ItemMetadata metaData)
        {
            if (!_loadedData.TryGetValue(metaData.Group, out Dictionary<int, EquipmentData> dataSet))
            {
                _loadedData.Add(metaData.Group, new Dictionary<int, EquipmentData>());
            }
            else if (dataSet.TryGetValue(metaData.Index, out EquipmentData data))
            {
                return data;
            }

            var loadedResource = Resources.Load<EquipmentData>($"Items/Equipments/{metaData.Group}/{metaData.Index}");
            if (loadedResource == null) return null;

            _loadedData[metaData.Group].Add(metaData.Index, loadedResource);
            loadedResource.Id = System.Guid.NewGuid();
            return loadedResource;
        }
    }
}
