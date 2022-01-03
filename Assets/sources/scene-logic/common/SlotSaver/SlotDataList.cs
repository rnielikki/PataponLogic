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
            for (i = 0; i < Core.Global.Slots.SlotMetaList.SlotMeta.Length; i++)
            {
                if (_items.Length <= i) break;
                _items[i].Init(Core.Global.Slots.SlotMetaList.SlotMeta[i], _scrollList, i, _onEntered);
            }
            _scrollList.Init(_items[0]);
            _scrollList.SetMaximumScrollLength(i - 1, _items[0]);
            _items[0].Selectable.Select();
        }
        private void OnEnable()
        {
            if (_items == null) return;
            foreach (var item in _items)
            {
                item.Selectable.enabled = true;
            }
            _items[0].Selectable.Select();
            _items[0].OnSelect(null);
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
