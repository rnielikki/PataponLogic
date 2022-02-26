using PataRoad.Common.Navigator;
using PataRoad.Core.Items;
using PataRoad.SceneLogic.CommonSceneLogic;
using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.EquipmentScene
{
    //dirty, I know, but can't even.
    public class EquipmentDisplay : MonoBehaviour
    {
        private SpriteNavigator _nav;
        [SerializeField]
        private CharacterGroupNavigator _groupNav;
        [SerializeField]
        private GameObject _child;

        private Core.Character.PataponData _currentPataponData;
        [SerializeField]
        private EquipmentSetter _equipmentSetter;

        [SerializeField]
        private RareponSelector _rareponSelector;

        private HeadquarterSummaryElement _summaryElem;

        [SerializeField]
        private HorizontalLayoutGroup _layout;

        [SerializeField]
        private InventoryDisplay _inventoryDisplay;

        [Header("Sound")]
        [SerializeField]
        private AudioClip _soundOpen;
        [SerializeField]
        private AudioClip _soundSelected;
        [SerializeField]
        private AudioClip _soundClose;

        private void Awake()
        {
            _layout = GetComponent<HorizontalLayoutGroup>();
        }
        public void ShowEquipment(Object sender, UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            _nav = sender as SpriteNavigator;

            _currentPataponData = _nav?.Current?.GetComponent<Core.Character.PataponData>();
            if (_currentPataponData == null) return;


            if (_nav is CharacterNavigator characterNavigator)
            {
                var elem = characterNavigator.EquipmentSummary.Current;
                characterNavigator.EquipmentSummary.SetInactive();

                if (elem.Item is Core.Character.Equipments.RareponData rarepon)
                {
                    _rareponSelector.Open(_nav, _currentPataponData, rarepon);
                    return;
                }
                var itemGroup = LoadItemType(_currentPataponData, elem);

                bool isEquipment = itemGroup.type == ItemType.Equipment;
                IItem item = isEquipment ? ItemLoader.GetItem(itemGroup.type, itemGroup.group, 0) : null;

                gameObject.SetActive(true);
                SetPosition(_currentPataponData.IndexInGroup < 2);
                if (isEquipment)
                {
                    _inventoryDisplay.AddItem(item);
                }
                else
                {
                    _inventoryDisplay.AddItem(null);
                }
                SetAppear(itemGroup.type, itemGroup.group, elem.Item, isEquipment);
            }
        }
        public void ShowItem(HeadquarterSummaryElement elem)
        {
            gameObject.SetActive(true);

            _summaryElem = elem;
            _nav = _groupNav;

            _groupNav.HeadquarterMenu.SetInactive();

            SetPosition(false);

            //add empty
            _inventoryDisplay.AddItem(null);

            SetAppear(ItemType.Key, elem.AdditionalData);
        }
        private void SetAppear(ItemType type, string itemGroup, IItem currentItem = null, bool isEquipments = false)
        {
            _inventoryDisplay.enabled = true;
            _nav.Freeze();

            Core.Global.GlobalData.Sound.PlayInScene(_soundOpen);
            _inventoryDisplay.SelectLast(
                _inventoryDisplay.LoadData(Core.Global.GlobalData.CurrentSlot.Inventory.GetItemsByType(type, itemGroup), currentItem, isEquipments)
            );
        }
        public void HideEquipment() => HideEquipment(_currentPataponData != null);
        private void HideEquipment(bool wasFromEquipmentSummary, bool equipped = false)
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            _nav.Defrost();

            Core.Global.GlobalData.Sound.PlayInScene(equipped ? _soundSelected : _soundClose);

            gameObject.SetActive(false);

            foreach (Transform item in _child.transform)
            {
                Destroy(item.gameObject);
            }
            if (wasFromEquipmentSummary) (_nav as CharacterNavigator)?.EquipmentSummary?.ResumeToActive();
            else (_nav as CharacterGroupNavigator)?.HeadquarterMenu?.ResumeToActive();
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
                    Core.Global.GlobalData.CurrentSlot.PataponInfo.UpdateGeneralEquipmentStatus(_currentPataponData.Type, item as GeneralModeData);
                }
                wasFromEquipmentSummary = true;
            }
            else
            {
                StringKeyItemData stringItem = item as StringKeyItemData;
                if (_summaryElem.AdditionalData == "Boss")
                {
                    Core.Global.GlobalData.CurrentSlot.PataponInfo.BossToSummon = stringItem;
                }
                else if (_summaryElem.AdditionalData == "Music")
                {
                    Core.Global.GlobalData.CurrentSlot.PataponInfo.CustomMusic = stringItem;
                }
                _summaryElem.transform.Find("Image").GetComponent<Image>().sprite = item?.Image;
                _summaryElem.GetComponentInChildren<Text>().text = item?.Name ?? "None";
            }
            HideEquipment(wasFromEquipmentSummary, true);
        }
        private (ItemType type, string group) LoadItemType(Core.Character.PataponData data, EquipmentSummaryElement equipElement)
        {
            if (equipElement.IsGeneralMode) return (ItemType.Key, "GeneralMode");
            else return (ItemType.Equipment, Core.Character.Class.ClassAttackEquipmentData.GetEquipmentName(data.Type, equipElement.EquipmentType));
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
