using PataRoad.Core.Items;
using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis.ItemExchange
{
    public class ItemExchangeWindow : SelectionWindow
    {
        [SerializeField]
        AudioClip _kaChingSound;
        [SerializeField]
        InventoryRefresher _inventoryStatus;
        ItemExchangeToggleMenu[] _menu;
        private IEnumerable<Core.Character.Class.ClassType> _availableClasses;
        private void Awake()
        {
            _menu = GetComponentsInChildren<ItemExchangeToggleMenu>(true);
        }
        private void Start()
        {
            _menu[0].ToggleToThis();
        }
        protected override void InitButtons()
        {
        }
        protected override void ResetButtons()
        {
        }
        public void Exchange(ExchangeMaterialSelection data)
        {
            if (!Core.Global.GlobalData.CurrentSlot.Inventory.HasAmountOfItem(data.InputItem, data.AmountRequirement))
            {
                Core.Global.GlobalData.Sound.PlayBeep();
                Common.GameDisplay.ConfirmDialog.Create("Item is insufficient")
                    .HideOkButton()
                    .SetTargetToResume(this)
                    .SelectCancel();
            }
            else
            {
                Common.GameDisplay.ConfirmDialog.Create("Do you want to exchange "
                    + $"from {data.InputItem.Name} x{data.AmountRequirement} to {data.OutputItem.Name}?")
                .SetTargetToResume(this)
                .SetOkAction(StartExchanging)
                .SelectOk();
            }
            void StartExchanging()
            {
                var currentSlot = Core.Global.GlobalData.CurrentSlot;
                var inventory = currentSlot.Inventory;

                inventory.RemoveItem(data.InputItem, data.AmountRequirement);
                inventory.AddItem(data.OutputItem);
                currentSlot.ExchangeRates.RaiseRate(data.Material, data.InputItem.Index);
                //gem
                if (data.InputItem.ItemType == ItemType.Equipment && data.InputItem is EquipmentData eq)
                {
                    var diff = inventory.GetAmount(data.InputItem)
                        - currentSlot.PataponInfo.GetEquippedCount(eq);
                    if (diff < 0)
                    {
                        if (_availableClasses == null)
                        {
                            _availableClasses = Core.Global.Slots.SlotStatusReader.GetAvailableClasses();
                        }
                        while (diff < 0)
                        {
                            currentSlot.PataponInfo.RemoveEquipment(_availableClasses, eq);
                            diff++;
                        }
                    }
                }
                Core.Global.GlobalData.Sound.PlayInScene(_kaChingSound);
                data.UpdateText();
                _inventoryStatus.Refresh();
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(data.gameObject);
            }
        }

        internal void UnToggleOthers()
        {
            foreach (var menu in _menu)
            {
                menu.UnToggle();
            }
        }
    }
}