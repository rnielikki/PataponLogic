using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis
{
    internal class InventoryTab : MonoBehaviour
    {
        [SerializeField]
        private InventoryLoader _inventoryLoader;
        private int _index;
        private InventoryTabElement[] _allTabElements;
        private int _tabCount;
        private InventoryTabElement _current => _allTabElements[_index];
        private void Start()
        {
            _allTabElements = GetComponentsInChildren<InventoryTabElement>();
            _tabCount = _allTabElements.Length;
        }
        public void MoveTo(Object sender, UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            var value = context.ReadValue<float>();
            if (value != 0) MoveTo(value > 0);
        }
        private void MoveTo(bool next)
        {
            if (next && _index < _tabCount - 1)
            {
                _current.Deselect();
                _index++;
            }
            else if (!next && _index > 0)
            {
                _current.Deselect();
                _index--;
            }
            else return;

            _current.Select(_inventoryLoader);
        }
    }
}
