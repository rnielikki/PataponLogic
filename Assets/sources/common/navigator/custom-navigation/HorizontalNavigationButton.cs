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
        public override Selectable FindSelectableOnLeft()
        {
            return _left;
        }
        public override Selectable FindSelectableOnRight()
        {
            return _right;
        }
        public override Selectable FindSelectableOnDown()
        {
            return _group.GetButtonOnNext(_selfIndex);
        }
        public override Selectable FindSelectableOnUp()
        {
            return _group.GetButtonOnPrevious(_selfIndex);
        }

    }
}
