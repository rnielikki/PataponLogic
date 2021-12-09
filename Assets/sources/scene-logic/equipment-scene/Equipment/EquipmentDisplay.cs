using PataRoad.Common.Navigator;
using PataRoad.Core.Items;
using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.EquipmentScene
{
    //dirty, I know, but can't even.
    public class EquipmentDisplay : MonoBehaviour
    {
        private ActionEventMap _map;
        private SpriteNavigator _nav;
        [SerializeField]
        private CharacterGroupNavigator _groupNav;
        [SerializeField]
        private GameObject _child;

        [SerializeField]
        private EquipmentSummary _summaryForEquipment;
        [SerializeField]
        private HeadquarterMenu _summaryForHeadquarter;

        private Core.Character.PataponData _currentPataponData;

        [SerializeField]
        private GameObject _itemTemplate;
        private bool _doesNavPreserveIndex;

        private HeadquarterSummaryElement _summaryElem;
        private HorizontalLayoutGroup _layout;

        [SerializeField]
        private UnityEngine.Events.UnityEvent<IItem> _onItemSelected;

        // Start is called before the first frame update
        void Awake()
        {
            _map = GetComponent<ActionEventMap>();
            _map.enabled = false;
            _layout = GetComponent<HorizontalLayoutGroup>();
        }
        public void ShowEquipment(Object sender, UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            gameObject.SetActive(true);

            _nav = sender as SpriteNavigator;
            _doesNavPreserveIndex = false;

            _currentPataponData = _nav?.Current?.GetComponent<Core.Character.PataponData>();
            if (_currentPataponData == null) return;

            var elem = _summaryForEquipment.Current;
            _summaryForEquipment.SetInactive();

            SetPosition(_currentPataponData.IndexInGroup < 2);
            var itemGroup = LoadItemType(_currentPataponData, elem);
            SetAppear(itemGroup.type, itemGroup.group, elem.Item);
        }
        public void ShowItem(HeadquarterSummaryElement elem)
        {
            gameObject.SetActive(true);

            _summaryElem = elem;
            _nav = _groupNav;
            _doesNavPreserveIndex = true;

            _summaryForHeadquarter.SetInactive();

            SetPosition(false);

            //add empty
            var obj = Instantiate(_itemTemplate, _child.transform);
            obj.GetComponent<ItemDisplay>().InitEmpty();
            SetAppear(ItemType.Key, elem.AdditionalData);
        }
        private void SetAppear(ItemType type, string itemGroup, IItem currentItem = null)
        {
            _nav.PreserveIndexOnDeselected = true;
            _map.enabled = true;
            _nav.Freeze();

            ItemDisplay last = null;

            foreach (var inventoryData in Core.Global.GlobalData.Inventory.GetItemsByType(type, itemGroup))
            {
                var obj = Instantiate(_itemTemplate, _child.transform);
                var display = obj.GetComponent<ItemDisplay>();
                display.Init(inventoryData.Item, inventoryData.Amount);
                if (inventoryData.Item != null && inventoryData.Item == currentItem)
                {
                    obj.GetComponent<Selectable>().interactable = false;
                    obj.GetComponent<ItemDisplay>().MarkAsDisable();
                }
                else last = display;
            }
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(last?.gameObject);
            if (last == null)
            {
                _onItemSelected.Invoke(null);
            }
        }
        public void HideEquipment() => HideEquipment(_currentPataponData != null);
        private void HideEquipment(bool wasFromEquipmentSummary)
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            _nav.PreserveIndexOnDeselected = _doesNavPreserveIndex;
            _nav.Defrost();
            _map.enabled = false;
            gameObject.SetActive(false);

            foreach (Transform item in _child.transform)
            {
                Destroy(item.gameObject);
            }
            if (wasFromEquipmentSummary) _summaryForEquipment.ResumeToActive();
            else _summaryForHeadquarter.ResumeToActive();
            _currentPataponData = null;
            _summaryElem = null;
        }
        public void Equip()
        {
            bool wasFromEquipmentSummary = false;

            var item = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject
                ?.GetComponent<ItemDisplay>()?.Item;

            if (_currentPataponData != null)
            {
                var equipment = item as EquipmentData;
                if (equipment != null)
                {
                    Core.Global.GlobalData.PataponInfo.UpdateClassEquipmentStatus(_currentPataponData, equipment);
                }
                else if (item != null)
                {
                    //general mode update!
                    _currentPataponData
                        .GetComponent<Core.Character.Patapons.General.PataponGeneral>()
                        .EquipGeneralMode(item as GeneralModeData);
                }
                wasFromEquipmentSummary = true;
            }
            else
            {
                StringKeyItemData stringItem = item as StringKeyItemData;
                if (_summaryElem.AdditionalData == "Boss")
                {
                    Core.Global.GlobalData.PataponInfo.BossToSummon = stringItem;
                }
                else if (_summaryElem.AdditionalData == "Music")
                {
                    Core.Global.GlobalData.PataponInfo.CustomMusic = stringItem;
                }
                _summaryElem.transform.Find("Image").GetComponent<Image>().sprite = item?.Image;
                _summaryElem.GetComponentInChildren<Text>().text = item?.Name ?? "None";
            }
            HideEquipment(wasFromEquipmentSummary);
        }
        public void SelectItem(ItemDisplay item) => _onItemSelected.Invoke(item.Item);
        private (ItemType type, string group) LoadItemType(Core.Character.PataponData data, EquipmentSummaryElement equipElement)
        {
            if (equipElement.IsGeneralMode) return (ItemType.Key, "GeneralMode");
            else return (ItemType.Equipment, data.GetEquipmentName(equipElement.EquipmentType));
        }
        private void SetPosition(bool left)
        {
            if (left)
            {
                _layout.childAlignment = TextAnchor.LowerLeft;
                _layout.reverseArrangement = false;
            }
            else
            {
                _layout.childAlignment = TextAnchor.LowerRight;
                _layout.reverseArrangement = true;
            }
        }
    }
}
