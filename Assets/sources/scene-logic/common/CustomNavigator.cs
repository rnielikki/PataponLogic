using PataRoad.Core.Map;
using UnityEngine;

namespace PataRoad.SceneLogic.WorldMap
{
    class CustomNavigator : MonoBehaviour, Common.Navigator.ICustomNavigator
    {
        CustomNavigationGroup[] _itemGroup;
        int _currentItemGroupIndex;
        private void Start()
        {
            _itemGroup = GetComponentsInChildren<CustomNavigationGroup>();
            InitializeSelection();
        }
        private void OnEnable()
        {
            InitializeSelection();
        }
        public void InitializeSelection()
        {
            if (_itemGroup != null && _itemGroup.Length != 0)
            {
                MoveIndexTo(_itemGroup.Length - 1);
            }
        }
        private void MoveIndexTo(int index)
        {
            _currentItemGroupIndex = (index + _itemGroup.Length) % _itemGroup.Length;
            _itemGroup[_currentItemGroupIndex].Select();
        }
        public void Move(Object sender, UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            Vector2 dir = context.ReadValue<Vector2>();
            var xAbsolute = Mathf.Abs(dir.x);
            var yAbsolute = Mathf.Abs(dir.y);
            if (xAbsolute > yAbsolute)
            {
                if (dir.x > 0) _itemGroup[_currentItemGroupIndex].SelectNext();
                else _itemGroup[_currentItemGroupIndex].SelectBefore();
            }
            else if (yAbsolute > xAbsolute)
            {
                MoveIndexTo(_currentItemGroupIndex + ((xAbsolute < 0) ? -1 : 1));
            }
        }
        public void ChangeTarget(GameObject newTarget)
        {
            _itemGroup = newTarget.GetComponentsInChildren<CustomNavigationGroup>();
            InitializeSelection();
        }
    }
}
