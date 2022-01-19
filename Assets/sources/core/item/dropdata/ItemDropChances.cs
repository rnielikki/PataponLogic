namespace PataRoad.Core.Items
{
    [System.Serializable]
    public class ItemDropChances
    {
        [UnityEngine.SerializeField] //Hey do not change to serializereference
        ItemMetaData _itemData;
        public IItem Item => _itemData.ToItem();
        [UnityEngine.SerializeField]
        float _chance;
        public float Chance => _chance;
    }
}
