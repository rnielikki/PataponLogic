using UnityEngine;

namespace PataRoad.SceneLogic.WorldMap
{
    class ScrollList : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _viewportRectTransform;
        private RectTransform _rectTransform;
        private float _elementHeight;
        private bool _enabled;
        private int _maximumScrollLength;

        [SerializeField]
        RectTransform _scrollLine;
        [SerializeField]
        RectTransform _scroll;

        internal void Init(RectTransform anyElementRectTransform)
        {
            _rectTransform = GetComponent<RectTransform>();
            _elementHeight = anyElementRectTransform.rect.size.y;
            _enabled = true;
        }
        internal void SetMaximumScrollLength(int length)
        {
            _maximumScrollLength = length;
            var activeScroll = length >= 0;

            _scrollLine.gameObject.SetActive(activeScroll);
            _scroll.gameObject.SetActive(activeScroll);

            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        }
        internal void Scroll(RectTransform element, int index)
        {
            if (!_enabled) return;
            var height = _viewportRectTransform.rect.size.y;
            var elementPos = -element.anchoredPosition.y;


            if (height < _rectTransform.rect.size.y)
            {
                if (elementPos + _elementHeight > _rectTransform.anchoredPosition.y + height)
                {
                    var pos = _rectTransform.anchoredPosition;
                    pos.y = elementPos - height + _elementHeight;
                    _rectTransform.anchoredPosition = pos;
                }
                else if (elementPos - _elementHeight < _rectTransform.anchoredPosition.y)
                {
                    var pos = _rectTransform.anchoredPosition;
                    pos.y = elementPos;
                    _rectTransform.anchoredPosition = pos;
                }
            }
            else _rectTransform.anchoredPosition = Vector2.zero;
            if (_maximumScrollLength > -1)
            {
                var scrollPos = _scroll.anchoredPosition;
                if (_maximumScrollLength == 0)
                {
                    scrollPos.y = 0;
                }
                else
                {
                    scrollPos.y = -((float)index / _maximumScrollLength) * _scrollLine.rect.size.y;
                }
                _scroll.anchoredPosition = scrollPos;
            }
        }
    }
}
