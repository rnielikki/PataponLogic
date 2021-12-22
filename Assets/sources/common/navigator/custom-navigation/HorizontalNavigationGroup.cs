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
        private void Start()
        {
            if (!_useLateInit) LateInit();
        }
        public void LateInit()
        {
            var children = GetComponentsInChildren<HorizontalNavigationButton>();
            for (int i = 0; i < children.Length; i++)
            {
                var child = children[i];
                Selectable left, right;

                if (!_wrapAround)
                {
                    left = (i == 0) ? child : children[i - 1];
                    right = (i == children.Length - 1) ? child : children[i + 1];
                }
                else
                {
                    left = children[(i - 1 + children.Length) % children.Length];
                    right = children[(i + 1) % children.Length];
                }
                child.Init(left, right);
            }
        }

    }
}
