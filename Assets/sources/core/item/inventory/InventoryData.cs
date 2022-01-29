namespace PataRoad.Core.Items
{
    /// <summary>
    /// For serialization.
    /// </summary>
    [System.Serializable]
    public class InventoryData
    {
        [System.NonSerialized]
        private IItem _item;
        public IItem Item => _item;

        [UnityEngine.SerializeReference]
        ItemMetaData _itemMeta;
        public ItemMetaData ItemMeta => _itemMeta;
        [UnityEngine.SerializeField]
        int _amount;
        /// <summary>
        /// Do not edit this directly outside <see cref="Inventory"/>.
        /// </summary>
        public int Amount { get => _amount; internal set => _amount = value; }
        public InventoryData(IItem item, int amount)
        {
            _item = item;
            _itemMeta = new ItemMetaData(item);
            _amount = amount;
        }
        /// <summary>
        /// Assign <see cref="Item"/> from deserialized data.
        /// </summary>
        /// <returns>self.</returns>
        public InventoryData AssignItem()
        {
            _item = _itemMeta.ToItem();
            return this;
        }
    }
}
