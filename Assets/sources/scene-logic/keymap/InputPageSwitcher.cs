using UnityEngine;

namespace PataRoad.SceneLogic.KeymapSettings
{
    class InputPageSwitcher : MonoBehaviour
    {
        [SerializeField]
        ActionTabInitializer[] _pages;
        [SerializeField]
        AudioClip _pageSwitchingSound;
        private int _index;
        private int _pageLength;
        private ActionTabInitializer _page => _pages[_index];
        private void Start()
        {
            _pageLength = _pages.Length;
            SelectCurrent();
        }
        public void ToLeft()
        {
            _page.gameObject.SetActive(false);
            _index = (_index - 1 + _pageLength) % _pageLength;
            SelectCurrent();
            Core.Global.GlobalData.Sound.PlayInScene(_pageSwitchingSound);
        }
        public void ToRight()
        {
            _page.gameObject.SetActive(false);
            _index = (_index + 1) % _pageLength;
            SelectCurrent();
            Core.Global.GlobalData.Sound.PlayInScene(_pageSwitchingSound);
        }
        private void SelectCurrent()
        {
            _page.gameObject.SetActive(true);
            _page.Initialize();
        }
    }
}
