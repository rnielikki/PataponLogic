using PataRoad.Core.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.Patapolis.ItemExchange
{
    public class ExchangeMaterialSelection : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        private FromMaterialType _materialType;
        public FromMaterialType Material => _materialType;
        private IItem _inputItem;
        public IItem InputItem => _inputItem;
        private IItem _outputItem;
        public IItem OutputItem => _outputItem;
        [SerializeField]
        private Image _image;
        [SerializeField]
        private Text _text;
        [SerializeField]
        private Button _button;
        [SerializeField]
        ExchangeSelectionUpdater _statusUpdater;
        private bool _isGem;
        public bool IsGem => _isGem;
        public float AmountRatio { get; private set; }
        public int AmountRequirement { get; private set; }

        internal void Init(ItemExchangeToggleMenu menu, int index)
        {
            _button.onClick.RemoveAllListeners();
            _isGem = menu.IsGem;
            if (!IsGem)
            {
                gameObject.SetActive(true);
                _materialType = menu.Material;
                _inputItem = ItemLoader.GetItem(ItemType.Material, _materialType.ToString(), index);
                var outputMaterialType = (ToMaterialType)(int)menu.Material;
                _outputItem = ItemLoader.GetItem(ItemType.Material, outputMaterialType.ToString(), index);
                LoadExchangeRate();
            }
            else if (index > 0 && index < 4)
            {
                _inputItem = ItemLoader.GetItem(ItemType.Equipment, "Gem", index);
                _outputItem = ItemLoader.GetItem(ItemType.Equipment, "Gem", index + 3);

                AmountRatio = 3;
                AmountRequirement = 3;
            }
            else
            {
                gameObject.SetActive(false);
                return;
            }
            _image.sprite = _inputItem.Image;
            UpdateText();
            _button.onClick.AddListener(() => menu.Window.Exchange(this));
        }
        internal void UpdateText()
        {
            _text.text = Core.Global.GlobalData.CurrentSlot.Inventory.GetAmount(_inputItem).ToString();
            if (!IsGem) LoadExchangeRate();
        }
        private void LoadExchangeRate()
        {
            AmountRatio = Core.Global.GlobalData.CurrentSlot.ExchangeRates.GetRate(_materialType);
            AmountRequirement = Core.Global.GlobalData.CurrentSlot
                .ExchangeRates.GetAmount(_materialType);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _statusUpdater.Hide();
        }

        public void OnSelect(BaseEventData eventData)
        {
            _statusUpdater.UpdateDescription(this);
        }
    }
}