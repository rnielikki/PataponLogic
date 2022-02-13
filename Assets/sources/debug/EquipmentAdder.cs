using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.AppDebug
{
    class EquipmentAdder : MonoBehaviour
    {
        [SerializeField]
        Core.Items.EquipmentData[] _equipments;
        public void AddItem()
        {
            foreach (var eq in _equipments)
            {
                var realItem = ItemLoader.GetItem(ItemType.Equipment, eq.Group, eq.Index);
                Core.Global.GlobalData.CurrentSlot.Inventory.AddItem(realItem);
            }
        }
    }
}
