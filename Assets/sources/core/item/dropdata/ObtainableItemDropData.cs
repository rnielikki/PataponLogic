using UnityEngine;

namespace PataRoad.Core.Items
{
    [CreateAssetMenu(fileName = "obtainable-item", menuName = "ItemDrop/Obtainable")]
    public class ObtainableItemDropData : ItemDropData
    {
        // ------- by item
        [Header("Item Info")]
        [SerializeField]
        private bool _dropInRandomGroupAndIndex;
        [SerializeField]
        private bool _dropRandomIndex;
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
        public IItem Item => GetItem();

        private IItem GetItem()
        {
            if (_dropInRandomGroupAndIndex)
            {
                return ItemLoader.GetRandomItem(_itemType, _itemIndex, _maxItemIndex);
            }
            else if (_dropRandomIndex)
            {
                return ItemLoader.GetItemFromRandomIndex(_itemType, _itemGroup, _itemIndex, _maxItemIndex);
            }
            else
            {
                return ItemLoader.GetItem(_itemType, ItemGroup, _itemIndex);
            }
        }
    }
}
