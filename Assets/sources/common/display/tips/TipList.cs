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
        [Header("progress")]
        [SerializeField]
        RectTransform _progressBar;
        [SerializeField]
        TMPro.TextMeshProUGUI _progressText;
        bool _started;
        private TipListItem _currentItem;
        void Start()
        {
            //progress bar
            var openedTipsCount = Core.Global.GlobalData.CurrentSlot.Tips.OpenTipsCount;
            var allTipsCount = TipsCollection.AllTipsCount;
            _progressBar.anchorMax = new Vector2(openedTipsCount / allTipsCount, 1);
            _progressText.text = $"{openedTipsCount}/{allTipsCount}";

            //list
            TipListItem firstItem = null;
            int len = -1;
            foreach (var tip in Core.Global.GlobalData.CurrentSlot.Tips.GetAllOpenedTips())
            {
                var listItem = Instantiate(_itemTemplate, transform);
                listItem.Init(tip, ++len, _scrollList, _tipDisplay, _eventMap);
                if (firstItem == null) firstItem = listItem;
            }
            if (firstItem == null) throw new System.ArgumentException("Opened tip amount must be at least 1");
            _scrollList.Init(firstItem);
            _scrollList.SetMaximumScrollLength(len, firstItem);
            _currentItem = firstItem;
            _currentItem.Selectable.Select();
            _started = true;
        }
        private void OnEnable()
        {
            if (!_started) return;
            _currentItem.Selectable.Select();
        }
        private void OnDisable()
        {
            var lastSelect = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            if (lastSelect != null)
            {
                var tipItem = lastSelect.GetComponent<TipListItem>();
                if (tipItem != null) _currentItem = tipItem;
            }
        }
    }
}