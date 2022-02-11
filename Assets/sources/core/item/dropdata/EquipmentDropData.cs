using UnityEngine;

namespace PataRoad.Core.Items
{
    [CreateAssetMenu(fileName = "equipment-item", menuName = "ItemDrop/Equipment")]
    class EquipmentDropData : ObtainableItemDropData
    {
        [SerializeReference]
        bool _randomEquipment;
        [SerializeReference]
        [Header("only valid with random equipment")]
        Character.Class.ClassType[] _classees;
        protected override string ItemGroup => GetItemGroup();
        private void Awake()
        {
            _itemType = ItemType.Equipment;
        }
        private string GetItemGroup()
        {
            if (_randomEquipment)
            {
                return Character.Class.ClassAttackEquipmentData.GetRandomEquipmentName(_classees);
            }
            else return base.ItemGroup;
        }
    }
}
