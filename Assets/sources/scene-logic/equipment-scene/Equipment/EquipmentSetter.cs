﻿using PataRoad.Core.Character;
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
        private UnityEngine.Events.UnityEvent<bool> _onOptimized;
        public void SetEquipment(PataponData pataponData, EquipmentData equipment)
        {
            var realAmount = GlobalData.Inventory.GetAmount(equipment);
            var amount = GlobalData.PataponInfo.GetEquippedCount(equipment);
            if (equipment.Index != 0 && equipment.Type != Core.Character.Equipments.EquipmentType.Rarepon && realAmount <= amount)
            {
                Exchange(pataponData, equipment);
            }
            else
            {
                GlobalData.PataponInfo.UpdateClassEquipmentStatus(pataponData, equipment);
            }
        }

        public void Optimize()
        {
            foreach (var classType in GlobalData.PataponInfo.CurrentClasses)
            {
                OptimizeGroup(classType);
            }
            _onOptimized.Invoke(true);
        }
        public void OptimizeGroup(Object sender, UnityEngine.InputSystem.InputAction.CallbackContext context)
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
                if (equipmentType != Core.Character.Equipments.EquipmentType.Rarepon &&
                    !(equipmentType == Core.Character.Equipments.EquipmentType.Helm && !pataponData.EquipmentManager.CanEquipHelm())
                        )
                    OptimizeOne(equipmentType);
            }

            void OptimizeOne(Core.Character.Equipments.EquipmentType type)
            {
                var equipmentGroup = pataponData.GetEquipmentName(type);
                var currentIndex = pataponData.EquipmentManager.GetEquipmentData(type).Index;
                var index = GlobalData.Inventory.GetBestEquipmentIndex(equipmentGroup);
                while (index > currentIndex)
                {
                    var item = ItemLoader.GetItem<EquipmentData>(ItemType.Equipment, equipmentGroup, index);
                    var availableAmount = GlobalData.Inventory.GetAmount(item) - GlobalData.PataponInfo.GetEquippedCount(item);
                    if (availableAmount > 0)
                    {
                        GlobalData.PataponInfo.UpdateClassEquipmentStatus(pataponData, item);
                        return;
                    }
                    else if (GlobalData.PataponInfo.IsEquippedOutside(CharacterGroupSaver.AvailableClasses, item, out var result))
                    {
                        var oldPataponData = _characterGroupSaver.GetPataponDataInIndex(result.type, result.index);
                        GlobalData.PataponInfo.ExchangeClassEquipmentStatus(oldPataponData, pataponData, item);
                        return;
                    }
                    index--;
                }
            }
        }
        private void Exchange(PataponData pataponData, EquipmentData equipment)
        {
            //1. Load meta.
            var data = GlobalData.PataponInfo.GetExchangablePataponMetaData(CharacterGroupSaver.AvailableClasses, pataponData, equipment);
            //2. Get Patapon data from meta.
            var oldPataponData = _characterGroupSaver.GetPataponDataInIndex(data.type, data.index);
            //3. Exchange!
            GlobalData.PataponInfo.ExchangeClassEquipmentStatus(oldPataponData, pataponData, equipment);
        }
        private void OnDestroy()
        {
            _onOptimized.RemoveAllListeners();
        }
    }
}