namespace PataRoad.Core.Items
{
    /// <summary>
    /// For serialization.
    /// </summary>
    [System.Serializable]
    public class InventoryData
    {
        [UnityEngine.SerializeReference]
        ItemMetaData _itemMeta;
        public ItemMetaData ItemMeta => _itemMeta;
        [UnityEngine.SerializeField]
        int _amount;
        public int Amount => _amount;
        public InventoryData(System.Collections.Generic.KeyValuePair<IItem, int> pair)
        {
            _itemMeta = new ItemMetaData(pair.Key);
            _amount = pair.Value;
        }
    }
}
