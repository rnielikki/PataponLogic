using PataRoad.Core.Items;

namespace PataRoad.SceneLogic.Patapolis.Minigame
{
    class ClassMinigameSelectionButton : MinigameSelectionButton
    {
        [UnityEngine.SerializeField]
        Core.Character.Equipments.EquipmentType _equipmentType;
        [UnityEngine.SerializeField]
        Core.Character.Class.ClassType _classType;
        private void Start()
        {
            if (_equipmentType == Core.Character.Equipments.EquipmentType.Helm) return;

            var classKeyItem = ItemLoader.GetItem(
                ItemType.Key,
                "Class",
                Core.Character.Class.ClassData.GetItemIndexFromClass(_classType)
            );
            if (!Core.Global.GlobalData.CurrentSlot.Inventory.HasItem(classKeyItem))
            {
                gameObject.SetActive(false);
            }
        }
        public override (IItem item, int amount) GetReward(int levelSum, int minMaxDifference)
        {
            throw new System.NotImplementedException();
        }
    }
}
