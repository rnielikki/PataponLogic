using PataRoad.Common.Navigator;
using PataRoad.SceneLogic.CommonSceneLogic;
using UnityEngine;
using System.Linq;

namespace PataRoad.SceneLogic.Patapolis
{
    class InventoryLoader : MonoBehaviour
    {
        [SerializeField]
        InventoryDisplay _inventoryDisplay;

        private ItemDisplay[] _allDisplays;
        private ItemDisplay[] _currentOrder;
        private ItemDisplay[] _allDisplaysAlphabetOrder;

        private ActionEventMap _parentSelector;

        public void Init(ActionEventMap parentSelector)
        {
            var loadedDisplays = _inventoryDisplay.LoadData(Core.Global.GlobalData.Inventory.GetAllItems(), null, false);
            _allDisplays = loadedDisplays
                .GroupBy(display => display.Item.ItemType)
                .SelectMany(item => item.GroupBy(display => display.Item.Group))
                .SelectMany(item => item.OrderBy(display => display.Item.Index))
                .ToArray();
            _allDisplaysAlphabetOrder = loadedDisplays.OrderBy(display => display.Item.Name).ToArray();
            OrderToIndex();
            _parentSelector = parentSelector;
        }

        public void Open()
        {
            _parentSelector.enabled = false;
            _inventoryDisplay.transform.parent.gameObject.SetActive(true);
            _inventoryDisplay.gameObject.SetActive(true);
            _inventoryDisplay.SelectFirst(_allDisplays);
        }
        public void Close()
        {
            _parentSelector.enabled = true;
            _inventoryDisplay.transform.parent.gameObject.SetActive(false);
            _inventoryDisplay.gameObject.SetActive(false);
        }
        public void OrderToAlphabet() => OrderTo(_allDisplaysAlphabetOrder);
        public void OrderToIndex() => OrderTo(_allDisplays);

        private void OrderTo(ItemDisplay[] order)
        {
            if (_currentOrder == order) return;
            _currentOrder = order;
            var lastSelect = GetLastSelect();
            for (int i = 0; i < order.Length; i++)
            {
                order[i].transform.SetSiblingIndex(i);
            }
            if (lastSelect == null)
            {
                lastSelect = order[0];
            }
            Scroll(lastSelect);
        }
        public void ShowAll()
        {
            var lastSelect = GetLastSelect();
            foreach (var display in _currentOrder)
            {
                display.gameObject.SetActive(true);
            }
            if (lastSelect == null) lastSelect = _inventoryDisplay.GetComponentInChildren<ItemDisplay>(false);
            Scroll(lastSelect);
        }
        internal void FilterTo(Core.Items.ItemType itemType)
        {
            foreach (var display in _currentOrder)
            {
                display.gameObject.SetActive(display.Item.ItemType == itemType);
            }
            Scroll(_inventoryDisplay.GetComponentInChildren<ItemDisplay>(false));
        }
        private ItemDisplay GetLastSelect()
        {
            var lastSelect = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            if (lastSelect == null) return null; //destroyed gameobject. "?." doesn't work with destroyed gameobj :(
            return lastSelect.GetComponent<ItemDisplay>();
        }
        private void Scroll(ItemDisplay scrollTarget)
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(scrollTarget?.gameObject);
            _inventoryDisplay.ScrollList.ForceRebuildLayout();
            _inventoryDisplay.ScrollList.Scroll(scrollTarget);
        }
    }
}
