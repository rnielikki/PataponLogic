using PataRoad.Common.Navigator;
using PataRoad.SceneLogic.CommonSceneLogic;
using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis
{
    class InventoryLoader : MonoBehaviour
    {
        [SerializeField]
        InventoryDisplay _inventoryDisplay;
        [SerializeField]
        ActionEventMap _actionEventMap;
        EquipmentScene.ItemDisplay[] _allDisplays;

        private void Start()
        {
            _allDisplays = _inventoryDisplay.LoadData(Core.Global.GlobalData.Inventory.GetAllItems(), null, false);
        }
        public void Open()
        {
            _actionEventMap.enabled = false;
            _inventoryDisplay.gameObject.SetActive(true);
            _inventoryDisplay.SelectFirst(_allDisplays);
        }
        public void Close()
        {
            _actionEventMap.enabled = true;
            _inventoryDisplay.gameObject.SetActive(false);
        }
    }
}
