using PataRoad.Core.Items;
using UnityEngine.EventSystems;

namespace PataRoad.SceneLogic.Patapolis.Minigame
{
    class ClassMinigameSelectionButton : MinigameSelectionButton, UnityEngine.EventSystems.ISelectHandler
    {
        [UnityEngine.SerializeField]
        Core.Character.Equipments.EquipmentType _equipmentType;
        [UnityEngine.SerializeField]
        Core.Character.Class.ClassType _classType;
        [UnityEngine.SerializeField]
        TMPro.TextMeshProUGUI _labelField;
        private string _label;
        private void Awake()
        {
            if (_equipmentType == Core.Character.Equipments.EquipmentType.Helm)
            {
                _label = "Helm";
                return;
            }

            var classKeyItem = ItemLoader.GetItem(
                ItemType.Key,
                "Class",
                Core.Character.Class.ClassData.GetItemIndexFromClass(_classType)
            );
            if (!Core.Global.GlobalData.CurrentSlot.Inventory.HasItem(classKeyItem))
            {
                gameObject.SetActive(false);
            }
            else
            {
                _label = $"{Core.Character.Class.ClassAttackEquipmentData.GetEquipmentName(_classType, _equipmentType)} "
                    + $"({_classType}/{_equipmentType})";
            }
        }
        public override (IItem item, int amount) GetReward(int levelSum, int minMaxDifference)
        {
            throw new System.NotImplementedException();
        }
        public void OnSelect(BaseEventData eventData)
        {
            _labelField.text = _label;
        }
    }
}
