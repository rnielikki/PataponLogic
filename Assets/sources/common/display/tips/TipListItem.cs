using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PataRoad.Common.GameDisplay
{
    public class TipListItem : MonoBehaviour, IScrollListElement
    {
        [SerializeField]
        private Button _selectable;
        public Selectable Selectable => _selectable;

        [SerializeField]
        private RectTransform _rectTransform;
        public RectTransform RectTransform => _rectTransform;

        public int Index { get; private set; }
        [SerializeField]
        private Text _text;
        private ScrollList _scrollList;

        [SerializeField]
        private AudioClip _selectedSound;
        [SerializeField]
        private AudioClip _openSound;

        internal void Init(TipDisplayData data, int indexInList, ScrollList scrollList,
            TipDisplay tipDisplay, Navigator.ActionEventMap eventMap)
        {
            Index = indexInList;
            _text.text = $"{data.Index}. {data.Title}";
            _scrollList = scrollList;
            _selectable.onClick.AddListener(() =>
            {
                Core.Global.GlobalData.Sound.PlayInScene(_openSound);
                tipDisplay.gameObject.SetActive(true);
                tipDisplay.LoadTip(data);
                eventMap.enabled = false;
            });
        }

        public void OnSelect(BaseEventData eventData)
        {
            _scrollList.Scroll(this);
            Core.Global.GlobalData.Sound.PlayInScene(_selectedSound);
        }
    }
}