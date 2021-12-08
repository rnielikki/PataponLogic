using PataRoad.Core.Items;
using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class ItemDisplay : MonoBehaviour
    {
        [SerializeField]
        private Image _image;
        [SerializeField]
        private Text _text;
        private IItem _item;
        public IItem Item => _item;
        private int _amount;
        private void Awake()
        {
            _text = GetComponentInChildren<Text>();
        }
        public void Init(IItem item, int amount)
        {
            _item = item;
            _image.sprite = item.Image;
            _text.text = amount.ToString();
        }
        public void InitEmpty()
        {
            _item = null;
            _image.sprite = null;
            _text.transform.parent.gameObject.SetActive(false);
        }
        public void MarkAsDisable()
        {
            _image.color = GetComponent<Selectable>().colors.disabledColor;
        }
        private bool UpdateDisplay()
        {
            if (_amount < 1) return false;
            _amount--;
            _text.text = _amount.ToString();
            return true;
        }
    }
}
