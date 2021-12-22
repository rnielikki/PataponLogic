using UnityEngine.UI;

namespace PataRoad.Common.Navigator
{
    class HorizontalNavigationButton : Button
    {
        private Selectable _left;
        private Selectable _right;
        public void Init(Selectable left, Selectable right)
        {
            _left = left;
            _right = right;
        }
        public override Selectable FindSelectableOnLeft()
        {
            return _left;
        }
        public override Selectable FindSelectableOnRight()
        {
            return _right;
        }
    }
}
