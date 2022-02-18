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
        public void AddAllEquipments()
        {
            foreach (Core.Character.Class.ClassType cl in System.Enum.GetValues(typeof(Core.Character.Class.ClassType)))
            {
                var weaponName = Core.Character.Class.ClassAttackEquipmentData.GetEquipmentName(cl, Core.Character.Equipments.EquipmentType.Weapon);
                var protectorName = Core.Character.Class.ClassAttackEquipmentData.GetEquipmentName(cl, Core.Character.Equipments.EquipmentType.Protector);
                FindAndAddAll(weaponName);
                if (protectorName != null) FindAndAddAll(protectorName);
            }
            FindAndAddAll("Helm");
            void FindAndAddAll(string name)
            {
                for (int i = 1; i < 15; i++)
                {

                    Core.Global.GlobalData.CurrentSlot.Inventory.AddItem(
                        ItemLoader.GetItem(ItemType.Equipment, name, i)
                    );
                }
            }
        }
    }
}
