using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.Main.SettingScene
{
    class SettingMenu : MonoBehaviour
    {
        [SerializeField]
        MainMenuSelector _parent;
        [SerializeField]
        Selectable _defaultSelection;
        private Selectable[] _allSelectables;

        [SerializeField]
        DifficultySettingToggleGroup _toggleGroup;

        [SerializeField]
        Slider _musicSlider;
        [SerializeField]
        Slider _soundSlider;
        private Core.Global.Settings.SettingModel _currentSetting;
        private bool _isOpen;

        private bool _changed;

        private void Awake()
        {
            _allSelectables = GetComponentsInChildren<Selectable>();
        }
        public void Open()
        {
            gameObject.SetActive(true);
            _currentSetting = Core.Global.GlobalData.Settings.Copy();
            _toggleGroup.Init(_currentSetting);
            _musicSlider.value = _currentSetting.MusicVolume;
            _soundSlider.value = _currentSetting.SoundVolume;
            _isOpen = true;
            OnEnable();

        }
        private void OnEnable()
        {
            if (!_isOpen) return;
            foreach (var selectable in _allSelectables)
            {
                selectable.interactable = true;
            }
            _defaultSelection.Select();
        }
        private void OnDisable()
        {
            if (!_isOpen) return;
            foreach (var selectable in _allSelectables)
            {
                selectable.interactable = false;
            }
        }
        public void SetDifficulty(bool isOn)
        {
            if (isOn && _isOpen)
            {
                _currentSetting.SetDifficulty(_toggleGroup.GetValueFromToggle());
                _changed = true;
            }
        }
        public void SetMusicVolume(float volume)
        {
            _currentSetting.MusicVolume = volume;
            _changed = true;
        }
        public void SetSoundVolume(float volume)
        {
            _currentSetting.SoundVolume = volume;
            _changed = true;
        }
        public void Close(bool apply)
        {
            _parent.ResumeNavigation();
            if (apply)
            {
                Core.Global.GlobalData.Settings.UpdateData(_currentSetting);
            }
            else if (_changed)
            {
                Common.GameDisplay.ConfirmDialog.Create("The changed values won't be saved.\nDo you want to quit?")
                    .SetTargetToResume(this)
                    .SetOkAction(Close)
                    .SelectCancel();
                return;
            }
            Close();
        }
        private void Close()
        {
            gameObject.SetActive(false);
            _isOpen = false;
        }
    }
}
