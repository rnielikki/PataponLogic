using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.Core.Items
{
    public class ItemStatusUpdater : MonoBehaviour
    {
        Text _text;
        public int Amount { get; private set; }
        private GameObject _textContainer;
        public IItem Item { get; private set; }
        // Start is called before the first frame update
        void Awake()
        {
            _text = GetComponentInChildren<Text>(true);
            _textContainer = _text.transform.parent.gameObject;
            Amount = 1;
        }
        public void SetItem(IItem item)
        {
            Item = item;
            transform.Find("Image").GetComponent<Image>().sprite = item.Image;
        }
        public void IncreaseCount()
        {
            if (Amount < 2)
            {
                _textContainer.SetActive(true);
            }
            Amount++;
            _text.text = Amount.ToString();
        }
    }
}
