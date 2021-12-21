using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis
{
    class InventoryTabFilterElement : InventoryTabElement
    {
        [SerializeField]
        private bool _isAll;
        [SerializeField]
        private ItemType _itemType;

        internal override void Select(InventoryLoader inventoryLoader)
        {
            base.Select(inventoryLoader);
            if (!_isAll) inventoryLoader.FilterTo(_itemType);
            else inventoryLoader.ShowAll();
        }
    }
}
