using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages all item informations. Also can pass to the other data loader like <see cref="EquipmentDataLoader"/>.
/// </summary>
namespace PataRoad.Core.Items
{
    public static class ItemLoader
    {
        //"Loading everything on memory at first time will be better"
        private static readonly Dictionary<ItemType, Dictionary<string, Dictionary<int, IItem>>> _data = new Dictionary<ItemType, Dictionary<string, Dictionary<int, IItem>>>();

        public static void LoadAll()
        {
            LoadAll(ItemType.Equipment);
            LoadAll(ItemType.Material);
            LoadAll(ItemType.Key);
        }
        /// <summary>
        /// Get Item
        /// </summary>
        /// <param name="group">The group that belongs to, like equipment, material, key etc.</param>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <returns>Equipment data.</returns>
        public static IItem GetItem(ItemType type, string group, int index)
        {
            if (_data.TryGetValue(type, out var dataByType)
                && dataByType.TryGetValue(group, out var dataByGroup)
                && dataByGroup.TryGetValue(index, out IItem data))
            {
                return data;
            }
            else return null;
        }


        private static void LoadAll(ItemType type)
        {
            string dir;
            switch (type)
            {
                case ItemType.Equipment:
                    dir = "Equipments";
                    break;
                case ItemType.Key:
                    dir = "Keys";
                    break;
                case ItemType.Material:
                    dir = "Materials";
                    break;
                default:
                    throw new System.NotSupportedException($"The item type *{type}* doesn't exist");
            }
            var result = new Dictionary<string, Dictionary<int, IItem>>();
            foreach (var data in Resources.LoadAll<EquipmentData>($"Items/{dir}"))
            {
                if (!result.ContainsKey(data.Group))
                {
                    result.Add(data.Group, new Dictionary<int, IItem>());
                }
                if (int.TryParse(data.name, out int index))
                {
                    result[data.Group].Add(index, data);
                    data.Id = System.Guid.NewGuid();
                }
            }
            _data.Add(type, result);
        }

        /// <summary>
        /// Loads equpment data and also saves data if not loaded.
        /// </summary>
        /// <param name="metaData">The metadata of item, which includes basic info, including name and id etc.</param>
        /// <returns>Equipment data.</returns>
        /*
        public static EquipmentData GetEquipment(ItemMetadata metaData)
        {
            if (!_equipmentData.TryGetValue(metaData.Group, out Dictionary<int, EquipmentData> dataSet))
            {
                return null;
            }
            else if (dataSet.TryGetValue(metaData.Index, out EquipmentData data))
            {
                return data;
            }
            return null;
        }
        */
    }
}
