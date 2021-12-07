using UnityEngine;

namespace PataRoad.Core.Items
{
    [System.Serializable]
    public class ItemMetaData
    {
        [SerializeField]
        ItemType _type;
        [SerializeField]
        string _group;
        [SerializeField]
        int _index;
        public ItemMetaData(IItem item)
        {
            _type = item.ItemType;
            _group = item.Group;
            _index = item.Index;
        }
        public IItem ToItem() => ItemLoader.GetItem(_type, _group, _index);
    }
}
