using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Items
{
    public class Inventory
    {
        private Dictionary<IItem, int> _data = new Dictionary<IItem, int>();

        private readonly Dictionary<ItemType, Dictionary<string, Dictionary<int, (IItem item, int amount)>>> _indexed
            = new Dictionary<ItemType, Dictionary<string, Dictionary<int, (IItem item, int amount)>>>();

        [SerializeReference]
        private InventoryData[] _serializableData;

        const string _keyInPref = "inventory";
        const int _maxValue = 999;
        public Inventory()
        {
            if (!PlayerPrefs.HasKey(_keyInPref))
            {
                LoadDefault();
            }
            else
            {
                try
                {
                    Deserialize(PlayerPrefs.GetString(_keyInPref));
                }
                catch (System.Exception)
                {
                    Debug.Log("Error while loading item. loading default items...");
                    LoadDefault();
                }
            }
            UpdateAllItemIndexes();
        }
        public bool HasItem(IItem item) => _data.ContainsKey(item);
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
                var currentAmount = _data[item];
                if (!item.IsUniqueItem && currentAmount < _maxValue)
                {
                    _data[item] = Mathf.Min(_maxValue, currentAmount + amount);
                    UpdateItemIndex(item, amount);
                }
                return false;
            }
            else
            {
                if (item.IsUniqueItem) amount = 1;
                _data.Add(item, amount);
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
            if (item.IsUniqueItem || !HasItem(item) || _data[item] < amount) return false;

            _data[item] -= _data[item] - amount;
            if (_data[item] == 0) _data.Remove(item);
            SetNewAmountFromItemIndexes(item, amount);
            return true;
        }
        public IEnumerable<(IItem item, int amount)> GetItemsByType(ItemType type, string group)
        {
            if (!_indexed.ContainsKey(type) || !_indexed[type].ContainsKey(group)) return Enumerable.Empty<(IItem, int)>();
            else return _indexed[type][group].Select(item => item.Value);
        }
        void LoadDefault()
        {
            //PATA AND PON DRUM
            _data.Add(ItemLoader.GetItem(ItemType.Key, "Drum", 0), 1);
            _data.Add(ItemLoader.GetItem(ItemType.Key, "Drum", 1), 1);

            //SONGS
            _data.Add(ItemLoader.GetItem(ItemType.Key, "Song", 0), 1);

            //MEMORIES
            _data.Add(ItemLoader.GetItem(ItemType.Key, "Class", 0), 1);
            _data.Add(ItemLoader.GetItem(ItemType.Key, "Class", 2), 1);
            _data.Add(ItemLoader.GetItem(ItemType.Key, "Class", 4), 1);
            _data.Add(ItemLoader.GetItem(ItemType.Key, "Class", 7), 1);
            _data.Add(ItemLoader.GetItem(ItemType.Key, "Class", 8), 1);

            //Default equipments
            foreach (var item in ItemLoader.GetAllDefaultEquipments())
            {
                int amount;
                switch (item.Type)
                {
                    case Character.Equipments.EquipmentType.Rarepon:
                        amount = 27;
                        break;
                    case Character.Equipments.EquipmentType.Helm:
                        amount = 36;
                        break;
                    default:
                        amount = 3;
                        break;
                }
                _data.Add(item, amount);
            }
        }
        private void UpdateAllItemIndexes()
        {
            foreach (var item in _data.Select(kv => (kv.Key, kv.Value)))
            {
                UpdateItemIndex(item.Key, item.Value);
            }
        }
        private void UpdateItemIndex(IItem item, int amount)
        {
            if (!_indexed.ContainsKey(item.ItemType))
            {
                _indexed.Add(item.ItemType, new Dictionary<string, Dictionary<int, (IItem item, int amount)>>());
            }
            var dataByItemType = _indexed[item.ItemType];
            if (!dataByItemType.ContainsKey(item.Group))
            {
                dataByItemType.Add(item.Group, new Dictionary<int, (IItem item, int amount)>());
            }
            var dataByItemGroup = dataByItemType[item.Group];
            if (!dataByItemGroup.ContainsKey(item.Index))
            {
                dataByItemGroup.Add(item.Index, (item, amount));
            }
            else
            {
                dataByItemGroup[item.Index] = (item, Mathf.Min(dataByItemGroup[item.Index].amount + amount, 999));
            }
        }
        private void SetNewAmountFromItemIndexes(IItem item, int newAmount)
        {
            var group = _indexed[item.ItemType][item.Group];
            if (newAmount < 1)
            {
                group.Remove(item.Index);
            }
            else
            {
                group[item.Index] = (item, newAmount);
            }
        }
        public string Serialize()
        {
            _serializableData = _data.Select(kv => new InventoryData(kv)).ToArray();
            return JsonUtility.ToJson(this);
        }
        public void Deserialize(string raw)
        {
            var inventoryData = JsonUtility.FromJson<Inventory>(raw);
            _serializableData = inventoryData._serializableData;
            _data = _serializableData.ToDictionary(data => data.ItemMeta.ToItem(), data => data.Amount);
        }
    }
}
