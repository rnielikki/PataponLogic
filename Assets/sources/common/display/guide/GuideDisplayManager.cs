using UnityEngine;

namespace PataRoad.Common.GameDisplay
{
    class GuideDisplayManager : MonoBehaviour
    {
        GuideDisplay[] _displays;
        GuideDisplay _current;

        private void Start()
        {
            _displays = GetComponentsInChildren<GuideDisplay>(true);
            _current = GetComponentInChildren<GuideDisplay>(false);
        }

        public void ChangeDisplay(GuideDisplay display)
        {
            if (_displays == null || _current == display || System.Array.IndexOf(_displays, display) < 0) return;
            _current.gameObject.SetActive(false);
            display.gameObject.SetActive(true);
            _current = display;
        }
        public void HideDisplay() => gameObject.SetActive(false);
        public void ShowDisplay() => gameObject.SetActive(true);
    }
}
