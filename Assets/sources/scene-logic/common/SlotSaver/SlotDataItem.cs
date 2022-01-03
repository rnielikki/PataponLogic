using PataRoad.Common.GameDisplay;
using PataRoad.Core.Global.Slots;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.CommonSceneLogic
{
    class SlotDataItem : MonoBehaviour, IScrollListElement, IDeselectHandler
    {
        RectTransform _rectTransform;
        public RectTransform RectTransform => _rectTransform;
        ScrollList _scrollList;

        [SerializeField]
        Button _button;
        public Selectable Selectable => _button;
        [SerializeField]
        Image _highlightImage;


        [SerializeField]
        AudioClip _selectSound;

        [Header("Display texts")]
        [SerializeField]
        Text _index;
        [SerializeField]
        Text _almighty;
        [SerializeField]
        Text _lastMap;
        [SerializeField]
        Text _headquarters;
        [SerializeField]
        Text _lastSavedTime;
        [SerializeField]
        Text _playTime;

        UnityEngine.Events.UnityEvent<SlotDataItem> _events;

        private SlotMeta _meta;
        public SlotMeta SlotMeta => _meta;

        public int Index { get; private set; }

        public void Init(SlotMeta slotMeta, ScrollList scrollList,
            int index, UnityEngine.Events.UnityEvent<SlotDataItem> selectedEvents)
        {
            _rectTransform = GetComponent<RectTransform>();
            _scrollList = scrollList;
            Index = index;
            _events = selectedEvents;
            if (slotMeta != null) UpdateDisplay(slotMeta);
            _button.onClick.AddListener(() => _events.Invoke(this));
        }
        public void UpdateDisplay(SlotMeta meta)
        {
            _meta = meta;
            _index.text = meta.SlotIndex.ToString();
            _almighty.text = meta.AlmightyName;
            _lastMap.text = meta.LastMapName;
            _headquarters.text = meta.SquadStatus;
            _lastSavedTime.text = meta.LastSavedTime;
            _playTime.text = meta.PlayTime.ToString(@"hh\:mm\:ss");
        }
        public void OnSelect(BaseEventData eventData)
        {
            Core.Global.GlobalData.Sound.PlayInScene(_selectSound);
            _highlightImage.enabled = true;
            _scrollList.Scroll(this);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _highlightImage.enabled = false;
        }
        private void OnDisable()
        {
            if (_highlightImage.enabled)
            {
                OnDeselect(null);
            }
        }
    }
}
