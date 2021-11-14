using System;
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
        private static readonly Dictionary<ItemType, string[]> _groupIndexes = new Dictionary<ItemType, string[]>();
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
            var groupIndexes = new List<string>();
            foreach (var data in Resources.LoadAll<EquipmentData>($"Items/{dir}"))
            {
                if (!result.ContainsKey(data.Group))
                {
                    result.Add(data.Group, new Dictionary<int, IItem>());
                    groupIndexes.Add(data.Group);
                }
                if (int.TryParse(data.name, out int index))
                {
                    result[data.Group].Add(index, data);
                    data.Id = Guid.NewGuid();
                }
            }
            _data.Add(type, result);
            _groupIndexes.Add(type, groupIndexes.ToArray());
        }

        /// <summary>
        /// Get Random item.
        /// </summary>
        /// <param name="itemType">Type of the item, like equipment or material.</param>
        /// <param name="indexMin">Minimum index value.</param>
        /// <param name="indexMax">Maximum index value.</param>
        /// <returns>Item in specific item, in random group. If the "random group" doesn't have corresponding index, returns <c>null</c></returns>
        /// <note>This DOESN't check do index exist, and can return <c>null</c> when any item has corresponding index.</note>
        internal static IItem GetRandomItem(ItemType itemType, int indexMin, int indexMax)
        {
            if (indexMin < 0) return null;
            var groupIndexData = _groupIndexes[itemType];
            var groupData = _data[itemType];
            var groupRandom = UnityEngine.Random.Range(0, groupIndexData.Length - 1);
            var itemData = groupData[groupIndexData[groupRandom]];

            var random = UnityEngine.Random.Range(indexMin, indexMax);
            if (itemData.TryGetValue(random, out IItem item)) return item;
            else return null;
        }
    }
}
