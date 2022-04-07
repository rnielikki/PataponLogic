using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Items
{
    [System.Serializable]
    public class Inventory : Global.Slots.IPlayerData
    {
        private readonly Dictionary<ItemType, Dictionary<string, Dictionary<int, InventoryData>>> _indexed
            = new Dictionary<ItemType, Dictionary<string, Dictionary<int, InventoryData>>>();
        private Dictionary<IItem, InventoryData> _existingData = new Dictionary<IItem, InventoryData>();

        /// <summary>
        /// Recent data after saved. This won't be serialized.
        /// </summary>
        [System.NonSerialized]
        private List<IItem> _recentData = new List<IItem>();

        [SerializeReference]
        private InventoryData[] _serializableData;

        public const int MaxAmount = 999;
        internal static Inventory CreateNew()
        {
            var inventory = new Inventory();
            inventory.LoadDefault();
            return inventory;
        }
        /// <summary>
        /// Check if the item exists in the inventory.
        /// </summary>
        /// <param name="item">item to check if it's in inventory.</param>
        /// <note>This doesn't check <c>null</c> and throws exception if data is <c>null</c></note>
        /// <returns><c>true</c> if exists in inventory, otherwise <c>false</c>.</returns>
        public bool HasItem(IItem item) => _existingData.ContainsKey(item);
        /// <summary>
        /// Check if the item with specific amount exists in the inventory.
        /// </summary>
        /// <param name="item">item to check if it's in inventory.</param>
        /// <param name="amount">amount of item that expected to be exist.</param>
        /// <note>This doesn't check <c>null</c> and throws exception if data is <c>null</c></note>
        /// <returns><c>true</c> if exists in inventory, otherwise <c>false</c>.</returns>
        public bool HasAmountOfItem(IItem item, int amount) => _existingData.ContainsKey(item) && _existingData[item].Amount >= amount;
        /// <summary>
        /// Retrieves all items the player have from the inventory.
        /// </summary>
        /// <returns>The inventory data of all items from the inventory.</returns>
        public IEnumerable<InventoryData> GetAllItems() => _existingData.Values;
        /// <summary>
        /// Checks if the item is recently added item.
        /// </summary>
        /// <remarks>The recent added item list is cleaned every time after a mission.</remarks>
        /// <returns><c>true</c> if it's recently added, otherwise <c>false</c>.</returns>
        public bool IsRecentItem(IItem item) => _recentData.Contains(item);
        /// <summary>
        /// Clear the recent item. Expected to be called after mission success or minigame success.
        /// </summary>
        public void ClearRecent() => _recentData.Clear();
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
                //don't add index 0 equipment(default)
                if (item.ItemType == ItemType.Equipment && item.Index == 0)
                {
                    return false;
                }
                if (!item.IsUniqueItem)
                {
                    _recentData.Add(item);
                    UpdateItemIndex(item, amount);
                }
                return false;
            }
            else
            {
                if (item.IsUniqueItem) amount = 1;
                _recentData.Add(item);
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


            //MEMORIES
            AddItem(ItemLoader.GetItem(ItemType.Key, "Class", 0));
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
                dataByItemGroup[item.Index].Amount = Mathf.Min(dataByItemGroup[item.Index].Amount + amount, MaxAmount);
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
        public void Serialize()
        {
            _serializableData = _existingData.Values.ToArray();
        }
        public void Deserialize()
        {
            _existingData = _serializableData.ToDictionary(data => data.ItemMeta.ToItem(), data => data.AssignItem());
            UpdateAllItemIndexes();
        }
    }
}
