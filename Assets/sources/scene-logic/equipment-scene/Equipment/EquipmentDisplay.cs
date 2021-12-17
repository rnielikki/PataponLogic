using PataRoad.Common.Navigator;
using PataRoad.Core.Items;
using System.Linq;
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
        private EquipmentSetter _equipmentSetter;

        [SerializeField]
        private RareponSelector _rareponSelector;

        [SerializeField]
        private GameObject _itemTemplate;

        private HeadquarterSummaryElement _summaryElem;
        private HorizontalLayoutGroup _layout;

        [SerializeField]
        private UnityEngine.Events.UnityEvent<IItem> _onItemSelected;

        [Header("Sound")]
        [SerializeField]
        private AudioClip _soundOpen;
        [SerializeField]
        private AudioClip _soundSelected;
        [SerializeField]
        private AudioClip _soundClose;

        [Header("Scroll")]
        [SerializeField]
        private Common.GameDisplay.ScrollList _scrollList;
        internal Common.GameDisplay.ScrollList ScrollList => _scrollList;
        [SerializeField]
        private GridLayoutGroup _gridLayoutGroup;

        // Start is called before the first frame update
        void Awake()
        {
            _map = GetComponent<ActionEventMap>();
            _map.enabled = false;
            _layout = GetComponent<HorizontalLayoutGroup>();
            _scrollList.Init(_gridLayoutGroup.cellSize.y);
        }
        public void ShowEquipment(Object sender, UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            _nav = sender as SpriteNavigator;

            _currentPataponData = _nav?.Current?.GetComponent<Core.Character.PataponData>();
            if (_currentPataponData == null) return;

            var elem = _summaryForEquipment.Current;
            _summaryForEquipment.SetInactive();

            if (elem.Item is Core.Character.Equipments.Weapons.RareponData rarepon)
            {
                _rareponSelector.Open(_nav, _currentPataponData, rarepon);
                return;
            }

            var itemGroup = LoadItemType(_currentPataponData, elem);

            var obj = Instantiate(_itemTemplate, _child.transform);
            bool isEquipment = itemGroup.type == ItemType.Equipment;
            IItem item = isEquipment ? ItemLoader.GetItem(itemGroup.type, itemGroup.group, 0) : null;

            gameObject.SetActive(true);
            SetPosition(_currentPataponData.IndexInGroup < 2);
            if (isEquipment)
            {
                obj.GetComponent<ItemDisplay>().Init(item);
            }
            else
            {
                obj.GetComponent<ItemDisplay>().InitEmpty();
            }
            SetAppear(itemGroup.type, itemGroup.group, elem.Item, isEquipment);
        }
        public void ShowItem(HeadquarterSummaryElement elem)
        {
            gameObject.SetActive(true);

            _summaryElem = elem;
            _nav = _groupNav;

            _summaryForHeadquarter.SetInactive();

            SetPosition(false);

            //add empty
            var obj = Instantiate(_itemTemplate, _child.transform);
            obj.GetComponent<ItemDisplay>().InitEmpty();
            SetAppear(ItemType.Key, elem.AdditionalData);
        }
        private void SetAppear(ItemType type, string itemGroup, IItem currentItem = null, bool isEquipments = false)
        {
            _map.enabled = true;
            _nav.Freeze();

            Core.Global.GlobalData.Sound.PlayInScene(_soundOpen);

            foreach (var inventoryData in Core.Global.GlobalData.Inventory.GetItemsByType(type, itemGroup))
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
                obj.GetComponent<Selectable>().interactable = false;
                obj.MarkAsDisable();
            }
            var lastSelectable = allItemDisplays.LastOrDefault(item => item.GetComponent<Selectable>().interactable);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(
                    lastSelectable?.gameObject
                );
            _scrollList.SetMaximumScrollLength(0, lastSelectable);
        }
        public void HideEquipment() => HideEquipment(_currentPataponData != null);
        private void HideEquipment(bool wasFromEquipmentSummary, bool equipped = false)
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            _nav.Defrost();

            Core.Global.GlobalData.Sound.PlayInScene(equipped ? _soundSelected : _soundClose);

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

            var itemDisplay = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject
                ?.GetComponent<ItemDisplay>();
            var item = itemDisplay?.Item;

            if (_currentPataponData != null)
            {
                var equipment = item as EquipmentData;
                if (equipment != null)
                {
                    _equipmentSetter.SetEquipment(_currentPataponData, equipment);
                }
                else if (item != null)
                {
                    //general mode update!
                    Core.Global.GlobalData.PataponInfo.UpdateGeneralEquipmentStatus(_currentPataponData.Type, item as GeneralModeData);
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
            HideEquipment(wasFromEquipmentSummary, true);
        }
        public void SelectItem(ItemDisplay item) => _onItemSelected.Invoke(item.Item);
        private (ItemType type, string group) LoadItemType(Core.Character.PataponData data, EquipmentSummaryElement equipElement)
        {
            if (equipElement.IsGeneralMode) return (ItemType.Key, "GeneralMode");
            else return (ItemType.Equipment, Core.Character.Class.ClassMetaData.GetEquipmentName(data.Type, equipElement.EquipmentType));
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
