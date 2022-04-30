using UnityEngine.UI;

namespace PataRoad.Common.Navigator
{
    class HorizontalNavigationButton : Button
    {
        private Selectable _left;
        private Selectable _right;
        private HorizontalNavigationGroup _group;
        private int _selfIndex;
        public void Init(Selectable left, Selectable right, HorizontalNavigationGroup group, int selfIndex)
        {
            _left = left;
            _right = right;
            _group = group;
            _selfIndex = selfIndex;
        }
    }
}
