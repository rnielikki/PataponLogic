using PataRoad.Common.Navigator;
using PataRoad.Core.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.CommonSceneLogic
{
    class InventoryDisplay : MonoBehaviour
    {
        [SerializeField]
        private GameObject _child;

        [SerializeField]
        private GameObject _itemTemplate;

        [Header("Scroll")]
        [SerializeField]
        private Common.GameDisplay.ScrollList _scrollList;
        internal Common.GameDisplay.ScrollList ScrollList => _scrollList;
        [SerializeField]
        private GridLayoutGroup _gridLayoutGroup;

        [Header("Event")]
        [SerializeField]
        private UnityEngine.Events.UnityEvent<IItem> _onItemSelected;

        private ActionEventMap _map;

        private void Awake()
        {
            _map = GetComponent<ActionEventMap>();
            _map.enabled = false;
            _scrollList.Init(_gridLayoutGroup.cellSize.y);
        }
        private void OnEnable()
        {
            _map.enabled = true;
        }
        private void OnDisable()
        {
            _map.enabled = false;
        }
        public void SelectItem(ItemDisplay item) => _onItemSelected.Invoke(item.Item);

        /// <summary>
        /// Initializes inventory.
        /// </summary>
        /// <param name="allInventoryData"></param>
        /// <param name="currentItem"></param>
        /// <param name="isEquipments">Is Equipment display. If <c>true</c>, it'll automatically subtract from current equipments.</param>
        /// <returns>Array of <see cref="ItemDisplay"/> in the current inventory window.</returns>
        /// <note>This doesn't turn on or off the game object, which means that can be used for background loading.</note>
        public ItemDisplay[] LoadData(IEnumerable<InventoryData> allInventoryData, IItem currentItem = null, bool isEquipments = false)
        {
            foreach (var inventoryData in allInventoryData)
            {
                var obj = Instantiate(_itemTemplate, _child.transform);
                var display = obj.GetComponent<ItemDisplay>();

                var amount = inventoryData.Amount;
                if (isEquipments)
                {
                    amount -= Core.Global.GlobalData.PataponInfo.GetEquippedCount(inventoryData.Item as EquipmentData);
                }
                display.Init(inventoryData.Item, amount);
            }
            var allItemDisplays = GetComponentsInChildren<ItemDisplay>();

            foreach (var obj in allItemDisplays.Where(item => item.Item == currentItem && item.Item != null))
            {
                if (isEquipments && (obj.Item as EquipmentData).Index == 0) continue;
                obj.MarkAsDisable();
            }
            return allItemDisplays;
        }
        public void EmptyData()
        {
            foreach (var data in _child.GetComponentsInChildren<ItemDisplay>()) Destroy(data.gameObject);
        }
        public void SelectFirst(IEnumerable<ItemDisplay> allItemDisplays)
        {
            _scrollList.SetMaximumScrollLength(0, null);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(
                    allItemDisplays.FirstOrDefault(item => item.GetComponent<Selectable>().interactable)?.gameObject
                );
        }
        public void SelectLast(IEnumerable<ItemDisplay> allItemDisplays)
        {
            var lastSelectable = allItemDisplays.LastOrDefault(item => item.GetComponent<Selectable>().interactable);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(
                    lastSelectable?.gameObject
                );
            _scrollList.SetMaximumScrollLength(0, lastSelectable);
        }
        public void AddItem(IItem item)
        {
            //add empty
            var obj = Instantiate(_itemTemplate, _child.transform).GetComponent<ItemDisplay>();
            if (item == null) obj.InitEmpty();
            else obj.Init(item);
        }
    }
}
