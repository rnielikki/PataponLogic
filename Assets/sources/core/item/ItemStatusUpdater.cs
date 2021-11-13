using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.Core.Items
{
    public class ItemStatusUpdater : MonoBehaviour
    {
        Text _text;
        int _amount;
        private GameObject _textContainer;
        private IItem _item;
        // Start is called before the first frame update
        void Awake()
        {
            _text = GetComponentInChildren<Text>(true);
            _textContainer = _text.transform.parent.gameObject;
            _amount = 1;
        }
        public void SetItem(IItem item)
        {
            _item = item;
            transform.Find("Image").GetComponent<Image>().sprite = item.Image;
        }
        public void IncreaseCount()
        {
            if (_amount < 2)
            {
                _textContainer.SetActive(true);
            }
            _amount++;
            _text.text = _amount.ToString();
        }
    }
}
