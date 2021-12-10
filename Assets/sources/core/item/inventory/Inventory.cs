﻿using PataRoad.Core.Global;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Items
{
    public class Inventory : IPlayerData
    {
        private readonly Dictionary<ItemType, Dictionary<string, Dictionary<int, InventoryData>>> _indexed
            = new Dictionary<ItemType, Dictionary<string, Dictionary<int, InventoryData>>>();
        private Dictionary<IItem, InventoryData> _existingData = new Dictionary<IItem, InventoryData>();

        [SerializeReference]
        private InventoryData[] _serializableData;

        const int _maxValue = 999;

        public Inventory()
        {
            LoadDefault();
        }
        /// <summary>
        /// Check if the item exists in the inventory.
        /// </summary>
        /// <param name="item">item to check if it's in inventory.</param>
        /// <note>This doesn't check <c>null</c> and throws exception if data is <c>null</c></note>
        /// <returns><c>true</c> if exists in inventory, otherwise <c>false</c>.</returns>
        public bool HasItem(IItem item) => _existingData.ContainsKey(item);
        /// <summary>
        /// Adds an item to the inventory.
        /// </summary>
        ///<param name="item">The item to remove.</param>
        /// <returns><c>true</c> if new item, otherwise <c>false</c>.</returns>
        public bool AddItem(IItem item) => AddMultiple(item, 1);

        /// <summary>
        /// Adds more than one amount of item to the inventory.
        /// </summary>
        ///<param name="item">The item to remove.</param>
        ///<param name="amount">The amount to remove. Don't give negative value here.</param>
        /// <returns><c>true</c> if new item, otherwise <c>false</c>.</returns>
        public bool AddMultiple(IItem item, int amount)
        {
            if (amount < 0) throw new System.ArgumentException("amount cannot be less than zero.");
            if (HasItem(item))
            {
                if (!item.IsUniqueItem)
                {
                    UpdateItemIndex(item, amount);
                }
                return false;
            }
            else
            {
                if (item.IsUniqueItem) amount = 1;
                UpdateItemIndex(item, amount);
                return true;
            }
        }
        /// <summary>
        /// Removes an item (-1) from the inventory.
        /// </summary>
        ///<param name="item">The item to remove.</param>
        /// <returns><c>true</c> if removal is succeeded, otherwise <c>false</c>.</returns>
        public bool RemoveItem(IItem item) => RemoveItem(item, 1);
        /// <summary>
        /// Removes multiple items from the inventory.
        /// </summary>
        ///<param name="item">The item to remove.</param>
        ///<param name="amount">The amount to remove. Don't give negative value here.</param>
        /// <returns><c>true</c> if removal is succeeded, otherwise <c>false</c>.</returns>
        public bool RemoveItem(IItem item, int amount)
        {
            if (amount < 0) throw new System.ArgumentException("amount cannot be less than zero.");
            if (item.IsUniqueItem || !HasItem(item) || _existingData[item].Amount < amount) return false;

            var newAmount = _existingData[item].Amount - amount;
            SetNewAmountFromItemIndexes(item, newAmount);
            return true;
        }
        public IEnumerable<InventoryData> GetItemsByType(ItemType type, string group)
        {
            if (!_indexed.ContainsKey(type) || !_indexed[type].ContainsKey(group)) return Enumerable.Empty<InventoryData>();
            else return _indexed[type][group].Select(item => item.Value);
        }
        public IEnumerable<T> GetKeyItems<T>(string group) where T : IItem
        {
            return GetItemsByType(ItemType.Key, group).Select(item => (T)item.Item);
        }
        void LoadDefault()
        {
            //PATA AND PON DRUM
            AddItem(ItemLoader.GetItem(ItemType.Key, "Drum", 0));
            AddItem(ItemLoader.GetItem(ItemType.Key, "Drum", 1));

            //SONGS
            AddItem(ItemLoader.GetItem(ItemType.Key, "Song", 0));

            //MEMORIES
            AddItem(ItemLoader.GetItem(ItemType.Key, "Class", 0));

            //Weapon
            //Default should be always zero and shouldn't count how much it is.
            //AddMultiple(ItemLoader.GetItem(ItemType.Equipment, "Spear", 0), 3);
            //AddMultiple(ItemLoader.GetItem(ItemType.Equipment, "Helm", 0), 4);
            //AddMultiple(ItemLoader.GetItem(ItemType.Equipment, "Rarepon", 0), 3);

            //Test
            AddMultiple(ItemLoader.GetItem(ItemType.Equipment, "Spear", 1), 1);
            AddMultiple(ItemLoader.GetItem(ItemType.Equipment, "Helm", 1), 2);
            AddMultiple(ItemLoader.GetItem(ItemType.Equipment, "Rarepon", 1), 3);
            AddMultiple(ItemLoader.GetItem(ItemType.Equipment, "Rarepon", 2), 3);
            AddMultiple(ItemLoader.GetItem(ItemType.Key, "Boss", 0), 1);
            AddItem(ItemLoader.GetItem(ItemType.Key, "Class", 1));
            AddItem(ItemLoader.GetItem(ItemType.Key, "Class", 2));
            AddItem(ItemLoader.GetItem(ItemType.Key, "Class", 8));
        }
        public int GetAmount(IItem item) =>
           _existingData.ContainsKey(item) ? _existingData[item].Amount : 0;

        public int GetBestEquipmentIndex(string group)
        {
            if (!_indexed.ContainsKey(ItemType.Equipment)) return 0;
            var allEquipments = _indexed[ItemType.Equipment];
            if (!allEquipments.ContainsKey(group)) return 0;
            return allEquipments[group].Max(eq => eq.Key);
        }


        private void UpdateAllItemIndexes()
        {
            foreach (var item in _existingData.Values)
            {
                UpdateItemIndex(item.Item, item.Amount);
            }
        }
        //UPDATE MUST CALLED *AFTER* ITEM IS ADDED TO "_existingData".
        private void UpdateItemIndex(IItem item, int amount)
        {
            if (!_indexed.ContainsKey(item.ItemType))
            {
                _indexed.Add(item.ItemType, new Dictionary<string, Dictionary<int, InventoryData>>());
            }
            var dataByItemType = _indexed[item.ItemType];
            if (!dataByItemType.ContainsKey(item.Group))
            {
                dataByItemType.Add(item.Group, new Dictionary<int, InventoryData>());
            }
            var dataByItemGroup = dataByItemType[item.Group];

            if (!dataByItemGroup.ContainsKey(item.Index))
            {
                if (!_existingData.ContainsKey(item)) _existingData.Add(item, new InventoryData(item, amount));
                dataByItemGroup.Add(item.Index, _existingData[item]);
            }
            else
            {
                dataByItemGroup[item.Index].Amount = Mathf.Min(dataByItemGroup[item.Index].Amount + amount, _maxValue);
            }
        }
        private void SetNewAmountFromItemIndexes(IItem item, int newAmount)
        {
            var group = _indexed[item.ItemType][item.Group];
            if (newAmount < 1)
            {
                group.Remove(item.Index);
                _existingData.Remove(item);
            }
            else
            {
                group[item.Index].Amount = newAmount;
            }
        }
        public string Serialize()
        {
            _serializableData = _existingData.Values.ToArray();
            return JsonUtility.ToJson(this);
        }
        public void Deserialize()
        {
            _existingData = _serializableData.ToDictionary(data => data.ItemMeta.ToItem(), data => data.AssignItem());
            UpdateAllItemIndexes();
        }
    }
}
