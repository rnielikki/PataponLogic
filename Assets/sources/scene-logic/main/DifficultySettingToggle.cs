using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.Main.SettingScene
{
    class DifficultySettingToggle : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField]
        Toggle _toggle;
        [SerializeField]
        Core.Rhythm.Difficulty _difficulty;
        [SerializeField]
        DifficultySettingToggleGroup _parent;

        [SerializeField]
        Image _image;
        [SerializeField]
        Color _selectedColor;
        Color _notSelectedColor;
        [SerializeField]
        Color _selectedAndOnTextColor;

        [SerializeField]
        Sprite _selectedSprite;
        Sprite _notSelectedSprite;
        public Core.Rhythm.Difficulty Difficulty => _difficulty;
        public Selectable Selectable => _toggle;

        [SerializeField]
        Image _bg;
        [SerializeField]
        Text _text;

        private void Awake()
        {
            _notSelectedSprite = _image.sprite;
            _notSelectedColor = new Color(1, 1, 1, 0);
            ResetTextColor();
        }
        public void OnChecked(bool on)
        {
            if (on)
            {
                _parent.SetCheckedToggle(this);
                _image.sprite = _selectedSprite;
                _text.color = _selectedAndOnTextColor;
            }
            else
            {
                _image.sprite = _notSelectedSprite;
                ResetTextColor();
            }
        }
        public void ResetTextColor()
        {
            _text.color = Color.white;
        }
        public void SwitchOn(bool on)
        {
            _toggle.isOn = on;
        }
        public void OnSelect(BaseEventData eventData)
        {
            _bg.color = _selectedColor;
            if (_toggle.isOn) _text.color = _selectedAndOnTextColor;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _bg.color = _notSelectedColor;
            _text.color = Color.white;
        }
    }
}
