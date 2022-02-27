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
            //holy moly you get more than 999 same items from a level
            else if (Amount < Inventory.MaxAmount)
            {
                Amount++;
                _text.text = Amount.ToString();
            }
        }
    }
}
