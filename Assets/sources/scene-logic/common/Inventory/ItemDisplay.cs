using PataRoad.Core.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.CommonSceneLogic
{
    public class ItemDisplay : MonoBehaviour, Common.GameDisplay.IScrollListElement
    {
        [SerializeField]
        private Image _image;
        [SerializeField]
        private Text _text;
        [SerializeField]
        private Image _background;
        public Image Background => _background;
        [SerializeField]
        private Sprite _backgroundOnNew;

        private IItem _item;
        public IItem Item => _item;
        private InventoryDisplay _parent;
        private Common.GameDisplay.ScrollList _scrollList;
        public int Amount { get; private set; }

        [SerializeField]
        private RectTransform _rectTransform;
        public RectTransform RectTransform => _rectTransform;
        public int Index { get; private set; }
        [SerializeField]
        private Selectable _selectable;
        public Selectable Selectable => _selectable;
        private void Awake()
        {
            _parent = GetComponentInParent<InventoryDisplay>();
            if (_parent != null) _scrollList = _parent.ScrollList;
        }
        public void Init(IItem item, int amount)
        {
            _item = item;
            if (Core.Global.GlobalData.CurrentSlot.Inventory.IsRecentItem(item))
            {
                _background.sprite = _backgroundOnNew;
            }
            _image.sprite = item.Image;
            Amount = amount;
            _text.text = amount.ToString();
        }
        public void Init(IItem item)
        {
            if (item == null)
            {
                return;
            }
            _item = item;
            _image.sprite = item.Image;
            _text.transform.parent.gameObject.SetActive(false);
        }

        public void InitEmpty()
        {
            _item = null;
            _image.sprite = null;
            _text.transform.parent.gameObject.SetActive(false);
        }
        public void UpdateText(int amount)
        {
            Amount = amount;
            _text.text = amount.ToString();
        }
        public void MarkAsDisable()
        {
            var comp = GetComponent<Selectable>();
            comp.interactable = false;
            _image.color = comp.colors.disabledColor;
        }
        public void MarkAsEnable()
        {
            var comp = GetComponent<Selectable>();
            comp.interactable = true;
            _image.color = comp.colors.normalColor;
        }
        public void OnSelect(BaseEventData eventData)
        {
            if (_parent != null) _parent.SelectItem(this);
            if (_scrollList != null) _scrollList.Scroll(this);
        }
    }
}
