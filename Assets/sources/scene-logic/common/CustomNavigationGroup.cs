using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.WorldMap
{
    class CustomNavigationGroup : MonoBehaviour, Common.Navigator.ICustomNavigationGroup
    {
        private Button[] _allButtons;
        private int _index = -1;
        private Button _lastButton => _allButtons[_index];
        private void Start()
        {
            _allButtons = GetComponentsInChildren<Button>();
            _index = _allButtons.Length - 1;
            if (_allButtons.Length < 1)
            {
                enabled = false;
            }
        }
        public void Select()
        {
            if (_index < 0) return;
            _index = _allButtons.Length - 1;
            EventSystem.current.SetSelectedGameObject(_lastButton.gameObject);
        }
        public void SelectBefore()
        {
            if (_allButtons.Length > 1)
            {
                _index = (_index - 1 + _allButtons.Length) % _allButtons.Length;
                EventSystem.current.SetSelectedGameObject(_lastButton.gameObject);
            }
        }
        public void SelectNext()
        {
            if (_allButtons.Length > 1)
            {
                _index = (_index + 1) % _allButtons.Length;
                EventSystem.current.SetSelectedGameObject(_lastButton.gameObject);
            }
        }

    }
}
