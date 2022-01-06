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
        [SerializeField]
        GameObject _savingImage;
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
                var newValue = _toggleGroup.GetValueFromToggle();
                if (newValue != _currentSetting.Difficulty)
                {
                    _changed = true;
                }
                _currentSetting.SetDifficulty(newValue);
            }
        }
        public void SetMusicVolume(float volume)
        {
            if (volume != _currentSetting.MusicVolume) _changed = true;
            _currentSetting.MusicVolume = volume;
        }
        public void SetSoundVolume(float volume)
        {
            if (volume != _currentSetting.SoundVolume) _changed = true;
            _currentSetting.SoundVolume = volume;
        }
        public void Close(bool apply)
        {
            if (apply)
            {
                Core.Global.GlobalData.GlobalInputActions.DisableNavigatingInput();
                _savingImage.SetActive(true);
                Core.Global.GlobalData.Settings.UpdateData(_currentSetting);
                _savingImage.SetActive(false);
                Core.Global.GlobalData.GlobalInputActions.EnableNavigatingInput();
            }
            else if (_changed)
            {
                Common.GameDisplay.ConfirmDialog.Create("The changed values won't be saved.\nDo you want to quit?")
                    .SetTargetToResume(this)
                    .SetOkAction(Close)
                    .CallOkActionLater()
                    .SelectCancel();
                return;
            }
            Close();
        }
        private void Close()
        {
            _parent.ResumeNavigation();
            gameObject.SetActive(false);
            _isOpen = false;
            _changed = false;
        }
    }
}
