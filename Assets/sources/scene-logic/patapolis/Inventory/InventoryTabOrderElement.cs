using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis
{
    class InventoryTabOrderElement : InventoryTabElement
    {
        private enum OrderType
        {
            Name,
            Index
        }
        [SerializeField]
        private OrderType _orderType;
        internal override void Select(InventoryLoader inventoryLoader)
        {
            base.Select(inventoryLoader);
            switch (_orderType)
            {
                case OrderType.Name:
                    inventoryLoader.OrderToAlphabet();
                    break;
                case OrderType.Index:
                    inventoryLoader.OrderToIndex();
                    break;
            }
        }
    }
}
