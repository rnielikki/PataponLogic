using PataRoad.Core.Items;
using UnityEngine.EventSystems;

namespace PataRoad.SceneLogic.Patapolis.Minigame
{
    class ClassMinigameSelectionButton : MinigameSelectionButton, ISelectHandler
    {
        [UnityEngine.SerializeField]
        Core.Character.Equipments.EquipmentType _equipmentType;
        [UnityEngine.SerializeField]
        Core.Character.Class.ClassType _classType;
        [UnityEngine.SerializeField]
        TMPro.TextMeshProUGUI _labelField;
        private string _label;
        private string _itemGroupName;

        const int _maxMaterialLevel = 4;

        private void Awake()
        {
            if (_equipmentType == Core.Character.Equipments.EquipmentType.Helm)
            {
                _label = "Helm";
                _itemGroupName = "Helm";
                return;
            }

            var classKeyItem = ItemLoader.GetItem(
                ItemType.Key,
                "Class",
                Core.Character.Class.ClassData.GetClassMemoryItemIndex(_classType)
            );
            if (!Core.Global.GlobalData.CurrentSlot.Inventory.HasItem(classKeyItem))
            {
                gameObject.SetActive(false);
            }
            else
            {
                _itemGroupName = Core.Character.Class.ClassAttackEquipmentData.GetEquipmentName(_classType, _equipmentType);
                _label = $"{_itemGroupName} "
                    + $"({_classType}/{_equipmentType})";
            }
        }
        public override (IItem item, int amount) GetReward(int levelSum, int minMaxDifference)
        {
            if (_itemGroupName == null) return (null, 0);
            return (ItemLoader.GetItem(ItemType.Equipment, _itemGroupName, GetItemIndex(levelSum, minMaxDifference)), 1);
        }
        private int GetItemIndex(int levelSum, int minMaxDifference)
        {
            //stars and bars problem but with only one bar.
            int baseValue = levelSum + 1;
            for (int i = 0; i < levelSum; i++)
            {
                baseValue += GetPossibilities(i);
            }
            //consider as distance from center.
            minMaxDifference -= levelSum % 2;
            //if move a bar, one side is +1, oteher side is -1, results 2 difference.
            return baseValue + (minMaxDifference / 2);
        }
        private int GetPossibilities(int sum)
        {
            int allPossibilities = sum / 2;
            int excludes = sum - _maxMaterialLevel;
            if (excludes > 0)
            {
                allPossibilities -= excludes;
            }
            return allPossibilities;
        }
        public void OnSelect(BaseEventData eventData)
        {
            _labelField.text = _label;
        }
    }
}
