using UnityEngine;

namespace PataRoad.Common.GameDisplay
{
    public class TipList : MonoBehaviour
    {
        [SerializeField]
        TipDisplay _tipDisplay;
        [SerializeField]
        ScrollList _scrollList;
        [SerializeField]
        TipListItem _itemTemplate;
        [SerializeField]
        Navigator.ActionEventMap _eventMap;
        // Use this for initialization
        void Start()
        {

            TipListItem firstItem = null;
            int len = -1;
            foreach (var tip in Core.Global.GlobalData.CurrentSlot.Tips.GetAllOpenedTips())
            {
                var listItem = Instantiate(_itemTemplate, transform);
                listItem.Init(tip, ++len, _scrollList, _tipDisplay, _eventMap);
                if (firstItem == null) firstItem = listItem;
            }
            if (firstItem == null) throw new System.ArgumentException("Opened tip amount must be at least 0");
            _scrollList.Init(firstItem);
            _scrollList.SetMaximumScrollLength(len, firstItem);
            firstItem.Selectable.Select();
        }
    }
}