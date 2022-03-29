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
                return;
            }
            Core.Global.GlobalData.CurrentSlot.Inventory.RemoveItem(data.InputItem, data.AmountRequirement);
            Core.Global.GlobalData.CurrentSlot.Inventory.AddItem(data.OutputItem);
            Core.Global.GlobalData.CurrentSlot.ExchangeRates.RaiseRate(data.Material, data.InputItem.Index);
            Core.Global.GlobalData.Sound.PlayInScene(_kaChingSound);
            data.UpdateText();
            _inventoryStatus.Refresh();
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(data.gameObject);
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