using PataRoad.Common.GameDisplay;
using PataRoad.Core.Character;
using PataRoad.Core.Global;
using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class EquipmentSetter : MonoBehaviour
    {
        [SerializeField]
        private CharacterGroupSaver _characterGroupSaver;
        [SerializeField]
        private CharacterGroupNavigator _characterGroupNavigator;
        [SerializeField]
        private UnityEngine.Events.UnityEvent<bool> _onOptimized;
        [SerializeField]
        private AudioClip _optimizingSound;
        public void PlayOptimizingSound() => GlobalData.Sound.PlayInScene(_optimizingSound);
        public void SetEquipment(PataponData pataponData, EquipmentData equipment)
        {
            var realAmount = GlobalData.CurrentSlot.Inventory.GetAmount(equipment);
            var amount = GlobalData.CurrentSlot.PataponInfo.GetEquippedCount(equipment);
            if (equipment.Index != 0 && equipment.Type != Core.Character.Equipments.EquipmentType.Rarepon && realAmount <= amount)
            {
                Exchange(pataponData, equipment);
            }
            else
            {
                GlobalData.CurrentSlot.PataponInfo.UpdateClassEquipmentStatus(pataponData, equipment);
            }
        }

        public void Optimize()
        {
            ConfirmDialog.Create("All Patapons are equipped automatically. Are you sure to proceed?")
                .SetTargetToResume(_characterGroupNavigator).SetOkAction(OptimizeInAction)
                .SelectOk();
        }
        private void OptimizeInAction()
        {
            foreach (var classType in GlobalData.CurrentSlot.PataponInfo.CurrentClasses)
            {
                OptimizeGroup(classType);
            }
            _onOptimized.Invoke(true);
        }
        public void OptimizeGroup(Object sender, UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            ConfirmDialog.Create("All in this group are equipped automatically. Are you sure to proceed?")
                .SetTargetToResume(sender as CharacterNavigator)
                .SetOkAction(() => OptimizeGroupInAction(sender))
                .SelectOk();
        }
        private void OptimizeGroupInAction(Object sender)
        {
            OptimizeGroup((sender as Component).GetComponentInChildren<PataponData>().Type);
            _onOptimized.Invoke(false);
        }
        private void OptimizeGroup(Core.Character.Class.ClassType type)
        {
            foreach (PataponData data in _characterGroupSaver.GetGroup(type).GetComponentsInChildren<PataponData>())
            {
                OptimizeIndividual(data);
            }
        }
        private void OptimizeIndividual(PataponData pataponData)
        {
            foreach (var equipmentType in pataponData.EquipmentManager.GetAvailableEquipmentTypes)
            {
                if (!(equipmentType == Core.Character.Equipments.EquipmentType.Rarepon || equipmentType == Core.Character.Equipments.EquipmentType.Gem) &&
                    !(equipmentType == Core.Character.Equipments.EquipmentType.Helm && !pataponData.EquipmentManager.CanEquipHelm())
                        )
                    OptimizeOne(equipmentType);
            }

            void OptimizeOne(Core.Character.Equipments.EquipmentType type)
            {
                var equipmentGroup = Core.Character.Class.ClassAttackEquipmentData.GetEquipmentName(pataponData.Type, type);
                var currentData = pataponData.EquipmentManager.GetEquipmentData(type);
                var currentIndex = currentData.Index;
                var index = GlobalData.CurrentSlot.Inventory.GetBestEquipmentIndex(equipmentGroup);
                while (index > currentIndex)
                {
                    var item = ItemLoader.GetItem<EquipmentData>(ItemType.Equipment, equipmentGroup, index);
                    var availableAmount = GlobalData.CurrentSlot.Inventory.GetAmount(item) - GlobalData.CurrentSlot.PataponInfo.GetEquippedCount(item);
                    if (item.LevelGroup <= currentData.LevelGroup)
                    {
                        return;
                    }
                    else if (availableAmount > 0)
                    {
                        GlobalData.CurrentSlot.PataponInfo.UpdateClassEquipmentStatus(pataponData, item);
                        return;
                    }
                    else if (GlobalData.CurrentSlot.PataponInfo.IsEquippedOutside(CharacterGroupSaver.AvailableClasses, item, out var result))
                    {
                        var oldPataponData = _characterGroupSaver.GetPataponDataInIndex(result.type, result.index);
                        GlobalData.CurrentSlot.PataponInfo.ExchangeClassEquipmentStatus(oldPataponData, pataponData, item);
                        return;
                    }
                    index--;
                }
            }
        }
        private void Exchange(PataponData pataponData, EquipmentData equipment)
        {
            //1. Load meta.
            var data = GlobalData.CurrentSlot.PataponInfo.GetExchangablePataponMetaData(CharacterGroupSaver.AvailableClasses, pataponData, equipment);
            //2. Get Patapon data from meta.
            var oldPataponData = _characterGroupSaver.GetPataponDataInIndex(data.type, data.index);
            //3. Exchange!
            GlobalData.CurrentSlot.PataponInfo.ExchangeClassEquipmentStatus(oldPataponData, pataponData, equipment);
        }
        private void OnDestroy()
        {
            _onOptimized.RemoveAllListeners();
        }
    }
}
