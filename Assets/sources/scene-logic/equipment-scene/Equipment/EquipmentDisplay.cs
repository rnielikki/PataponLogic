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
        private GameObject _itemTemplate;

        private HeadquarterSummaryElement _summaryElem;
        private HorizontalLayoutGroup _layout;

        [SerializeField]
        private UnityEngine.Events.UnityEvent<IItem> _onItemSelected;

        [SerializeField]
        private AudioClip _soundOpen;
        [SerializeField]
        private AudioClip _soundClose;

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

            _currentPataponData = _nav?.Current?.GetComponent<Core.Character.PataponData>();
            if (_currentPataponData == null) return;

            var elem = _summaryForEquipment.Current;
            _summaryForEquipment.SetInactive();

            SetPosition(_currentPataponData.IndexInGroup < 2);
            var itemGroup = LoadItemType(_currentPataponData, elem);

            var obj = Instantiate(_itemTemplate, _child.transform);
            IItem item = (itemGroup.type == ItemType.Equipment) ? ItemLoader.GetItem(itemGroup.type, itemGroup.group, 0) : null;
            obj.GetComponent<ItemDisplay>().Init(item);
            SetAppear(itemGroup.type, itemGroup.group, elem.Item, true);
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
            _nav.PreserveIndexOnDeselected = true;
            _map.enabled = true;
            _nav.Freeze();

            Core.Global.GlobalData.Sound.Play(_soundOpen);

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
                obj.GetComponent<Selectable>().interactable = false;
                obj.MarkAsDisable();
            }
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(
                    allItemDisplays.LastOrDefault(item => item.GetComponent<Selectable>().interactable)?.gameObject
                );
        }
        public void HideEquipment() => HideEquipment(_currentPataponData != null);
        private void HideEquipment(bool wasFromEquipmentSummary)
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            _nav.Defrost();

            Core.Global.GlobalData.Sound.Play(_soundClose);

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
            else return (ItemType.Equipment, data.ClassMetaData.GetEquipmentName(equipElement.EquipmentType));
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
