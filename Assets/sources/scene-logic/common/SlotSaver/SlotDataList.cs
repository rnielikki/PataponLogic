using PataRoad.Core.Global.Slots;
using UnityEngine;

namespace PataRoad.SceneLogic.CommonSceneLogic
{
    class SlotDataList : MonoBehaviour
    {
        [SerializeField]
        private Common.GameDisplay.ScrollList _scrollList;
        private SlotDataItem[] _items;

        [SerializeField]
        UnityEngine.Events.UnityEvent<SlotDataItem> _onEntered;

        [SerializeField]
        UnityEngine.Events.UnityEvent _onClosed;

        private void Start()
        {
            _items = GetComponentsInChildren<SlotDataItem>();
            int i;
            for (i = 0; i < SlotMetaList.SlotMeta.Length; i++)
            {
                if (_items.Length <= i) break;
                _items[i].Init(SlotMetaList.SlotMeta[i], _scrollList, i, _onEntered);
            }
            var lastIndex = SlotMetaList.LastSlotIndex;
            _scrollList.Init(_items[lastIndex]);
            _scrollList.SetMaximumScrollLength(i - 1, _items[lastIndex]);
            _items[lastIndex].Selectable.Select();
        }
        private void OnEnable()
        {
            if (_items == null) return;
            foreach (var item in _items)
            {
                item.Selectable.enabled = true;
            }
            _items[SlotMetaList.LastSlotIndex].Selectable.Select();
            _items[SlotMetaList.LastSlotIndex].OnSelect(null);
        }
        private void OnDisable()
        {
            foreach (var item in _items)
            {
                item.Selectable.enabled = false;
            }
        }
        public void Close()
        {
            _onClosed.Invoke();
            gameObject.SetActive(false);
        }
    }
}
