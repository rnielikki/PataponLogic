﻿using System;
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
        private static readonly Dictionary<ItemType, Dictionary<string, Dictionary<int, IItem>>> _data
            = new Dictionary<ItemType, Dictionary<string, Dictionary<int, IItem>>>();
        private static readonly Dictionary<ItemType, string[]> _groupIndexes = new Dictionary<ItemType, string[]>();

        public static void LoadAll()
        {
            Load<EquipmentData>(ItemType.Equipment, "Equipments");
            Load<KeyItemData>(ItemType.Key, "Keys");
            Load<MaterialData>(ItemType.Material, "Materials");
        }
        private static void Load<T>(ItemType type, string path) where T : ScriptableObject, IItem
        {
            var result = new Dictionary<string, Dictionary<int, IItem>>();
            var groupIndexes = new List<string>();
            foreach (var data in Resources.LoadAll<T>($"Items/{path}"))
            {
                if (!result.ContainsKey(data.Group))
                {
                    result.Add(data.Group, new Dictionary<int, IItem>());
                    groupIndexes.Add(data.Group);
                }
                if (!result[data.Group].ContainsKey(data.Index))
                {
                    result[data.Group].Add(data.Index, data);
                    data.Id = Guid.NewGuid();
                }
                else
                {
                    throw new ArgumentException(
                        $"Index {data.Index} of {data.Group}/{data.Name} is already assigned for "
                        + $"{result[data.Group]}/{result[data.Group][data.Index].Name}"
                    );
                }
            }
            _groupIndexes.Add(type, groupIndexes.ToArray());
            _data.Add(type, result);
        }
        /// <summary>
        /// Gets (serializefield expected) reference to real item as in-game data.
        /// </summary>
        /// <param name="item">Item object, expected as scriptableobject.</param>
        /// <returns>Item in itemloader index.</returns>
        public static IItem LoadByReference(IItem item) => GetItem(item.ItemType, item.Group, item.Index);

        /// <summary>
        /// Gets item from type, group and index.
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
        public static T GetItem<T>(ItemType type, string group, int index) where T : IItem => (T)GetItem(type, group, index);

        /// <summary>
        /// Get Random item in RANDOM GROUP.
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
            var groupRandom = UnityEngine.Random.Range(0, groupIndexData.Length);

            return GetItemFromRandomIndex(itemType, groupIndexData[groupRandom], indexMax, indexMax);
        }
        /// <summary>
        /// Get Random item in specific group but same index.
        /// </summary>
        /// <param name="itemType">Type of the item, like equipment or material.</param>
        /// <param name="group">The item group (string).</param>
        /// <param name="indexMin">Minimum index value.</param>
        /// <param name="indexMax">Maximum index value.</param>
        /// <returns>Item in specific item, in random group. If the "random group" doesn't have corresponding index, returns <c>null</c></returns>
        /// <note>This DOESN't check do index exist, and can return <c>null</c> when any item has corresponding index.</note>
        internal static IItem GetItemFromRandomIndex(ItemType itemType, string group, int indexMin, int indexMax)
        {
            if (indexMin < 0) return null;
            var groupData = _data[itemType];
            var itemData = groupData[group];

            var random = UnityEngine.Random.Range(indexMin, indexMax + 1);
            if (itemData.TryGetValue(random, out IItem item)) return item;
            else return null;
        }
    }
}
