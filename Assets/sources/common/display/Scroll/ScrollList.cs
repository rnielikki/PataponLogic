﻿using UnityEngine;

namespace PataRoad.Common.GameDisplay
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
        private bool _useIndex = true;

        [SerializeField]
        RectTransform _scrollLine;
        [SerializeField]
        RectTransform _scroll;

        internal void Init(IScrollListElement anyElement) => Init(anyElement.RectTransform.rect.size.y);
        internal void Init(float cellHeight)
        {
            _rectTransform = GetComponent<RectTransform>();
            _elementHeight = cellHeight;
            _enabled = true;
        }
        internal void SetMaximumScrollLength(int length, IScrollListElement firstSelect)
        {
            if (_useIndex)
            {
                _maximumScrollLength = length;
            }

            var isScrollActive = length >= 0;
            _scrollLine.gameObject.SetActive(isScrollActive);
            _scroll.gameObject.SetActive(isScrollActive);

            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject((firstSelect as MonoBehaviour)?.gameObject);

            if (firstSelect == null)
            {
                SetToZero();
            }
            else
            {
                Scroll(firstSelect);
            }
        }
        internal void Scroll(IScrollListElement element) => Scroll(element.RectTransform, element.Index);
        private void Scroll(RectTransform rect, int index)
        {
            if (!_enabled) return;
            var height = _viewportRectTransform.rect.size.y;
            var elementPos = -rect.anchoredPosition.y;

            var contentHeight = _rectTransform.rect.size.y;
            var contentPosition = _rectTransform.anchoredPosition.y;

            if (height < contentHeight)
            {
                if (elementPos + _elementHeight > contentPosition + height)
                {
                    var pos = _rectTransform.anchoredPosition;
                    pos.y = elementPos - height + _elementHeight;
                    _rectTransform.anchoredPosition = pos;
                }
                else if (elementPos - _elementHeight < contentPosition)
                {
                    var pos = _rectTransform.anchoredPosition;
                    pos.y = elementPos;
                    _rectTransform.anchoredPosition = pos;
                }
                //must be updated position y value
                UpdateIndexByPosition(Mathf.InverseLerp(0, contentHeight - height, _rectTransform.anchoredPosition.y));
            }
            else
            {
                SetToZero();
            }

            if (_useIndex && _maximumScrollLength > -1)
            {
                if (_maximumScrollLength == 0)
                {
                    UpdateIndexByPosition(0);
                }
                else
                {
                    UpdateIndexByPosition((float)index / _maximumScrollLength);
                }
            }
        }
        private void SetToZero()
        {
            _rectTransform.anchoredPosition = Vector2.zero;
            if (!_useIndex) UpdateIndexByPosition(0);
        }
        private void UpdateIndexByPosition(float positionOffsetToScroll) //parameter is value of 0-1
        {
            var scrollPos = _scroll.anchoredPosition;
            scrollPos.y = -positionOffsetToScroll * _scrollLine.rect.size.y;
            _scroll.anchoredPosition = scrollPos;
        }
    }
}