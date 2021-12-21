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
        private IItem _item;
        public IItem Item => _item;
        private InventoryDisplay _parent;
        private Common.GameDisplay.ScrollList _scrollList;

        public RectTransform RectTransform { get; private set; }
        public int Index { get; private set; }
        public Selectable Selectable { get; private set; }
        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            Selectable = GetComponent<Selectable>();
            _text = GetComponentInChildren<Text>();
            _parent = GetComponentInParent<InventoryDisplay>();
            _scrollList = _parent.ScrollList;
        }
        public void Init(IItem item, int amount)
        {
            _item = item;
            _image.sprite = item.Image;
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
        public void UpdateText(int amount) => _text.text = amount.ToString();
        public void MarkAsDisable()
        {
            _image.color = GetComponent<Selectable>().colors.disabledColor;
        }

        public void OnSelect(BaseEventData eventData)
        {
            _parent.SelectItem(this);
            _scrollList.Scroll(this);
        }
    }
}
