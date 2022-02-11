using UnityEngine;

namespace PataRoad.Core.Items
{
    [CreateAssetMenu(fileName = "obtainable-item", menuName = "ItemDrop/Obtainable")]
    public class ObtainableItemDropData : ItemDropData
    {
        // ------- by item
        [Header("Item Info")]
        [SerializeField]
        private bool _dropRandomItem;
        [SerializeReference]
        protected ItemType _itemType;
        [SerializeReference]
        string _itemGroup;
        protected virtual string ItemGroup => _itemGroup;
        [SerializeReference]
        int _itemIndex;
        [SerializeReference]
        [Tooltip("This is only meaningful when item drop is random.")]
        int _maxItemIndex;
        [SerializeField] //Hey do not change to serializereference
        private ItemDropChances[] _itemDropChances;
        public System.Collections.Generic.IEnumerable<ItemDropChances> ItemDropChances => _itemDropChances;
        public IItem Item => (_dropRandomItem) ? ItemLoader.GetRandomItem(_itemType, _itemIndex, _maxItemIndex) : ItemLoader.GetItem(_itemType, ItemGroup, _itemIndex);
    }
}
