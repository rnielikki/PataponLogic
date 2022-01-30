using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.Common.Navigator
{
    /// <summary>
    /// Automatically matches left-right element inside. Works with horizontally arranged group like horizontal layout group.
    /// </summary>
    class HorizontalNavigationGroup : MonoBehaviour
    {
        [SerializeField]
        bool _useLateInit;
        [SerializeField]
        bool _wrapAround;
        private HorizontalNavigationButton[] _children;
        private HorizontalNavigationGroup _prevGroup;
        private HorizontalNavigationGroup _nextGroup;
        private void Start()
        {
            if (!_useLateInit) LateInit();
        }
        public void LateInit()
        {
            SetSiblings();
            _children = GetComponentsInChildren<HorizontalNavigationButton>(false);
            for (int i = 0; i < _children.Length; i++)
            {
                var child = _children[i];
                Selectable left, right;

                if (!_wrapAround)
                {
                    left = (i == 0) ? child : _children[i - 1];
                    right = (i == _children.Length - 1) ? child : _children[i + 1];
                }
                else
                {
                    left = _children[(i - 1 + _children.Length) % _children.Length];
                    right = _children[(i + 1) % _children.Length];
                }
                child.Init(left, right, this, i);
            }
        }
        public Button GetButtonOnPrevious(int index) => GetButtonInIndex(index, _prevGroup);
        public Button GetButtonOnNext(int index) => GetButtonInIndex(index, _nextGroup);
        private Button GetButtonInIndex(int index, HorizontalNavigationGroup group)
        {
            if (group == null)
            {
                return null;
            }
            else if (group._children.Length == 0)
            {
                return GetButtonOnNext(index);
            }
            if (index >= 0 && index < group._children.Length)
            {
                return group._children[index];
            }
            else
            {
                return group._children[group._children.Length - 1];
            }
        }
        private void SetSiblings()
        {
            var allGroups = transform.parent.GetComponentsInChildren<HorizontalNavigationGroup>(false);
            var index = System.Array.IndexOf(allGroups, this);
            var prevIndex = index - 1;
            var nextIndex = index + 1;
            if (prevIndex < 0)
            {
                if (!_wrapAround)
                {
                    _prevGroup = null;
                }
                else
                {
                    _prevGroup = allGroups[allGroups.Length - 1];
                }
            }
            else
            {
                _prevGroup = allGroups[prevIndex];
            }
            if (nextIndex >= allGroups.Length)
            {
                if (!_wrapAround)
                {
                    _nextGroup = null;
                }
                else
                {
                    _nextGroup = allGroups[0];
                }
            }
            else
            {
                _nextGroup = allGroups[nextIndex];
            }
        }
    }
}
