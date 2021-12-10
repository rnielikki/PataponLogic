using PataRoad.Core.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class ItemDisplay : MonoBehaviour, ISelectHandler
    {
        [SerializeField]
        private Image _image;
        [SerializeField]
        private Text _text;
        private IItem _item;
        public IItem Item => _item;
        private int _amount;
        private EquipmentDisplay _parent;
        private void Awake()
        {
            _text = GetComponentInChildren<Text>();
            _parent = GetComponentInParent<EquipmentDisplay>();
        }
        public void Init(IItem item, int amount)
        {
            _item = item;
            _image.sprite = item.Image;
            _text.text = amount.ToString();
        }
        public void Init(IItem item)
        {
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
        }
    }
}
