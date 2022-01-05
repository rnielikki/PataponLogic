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
        Sprite _selectedSprite;
        Sprite _notSelectedSprite;
        public Core.Rhythm.Difficulty Difficulty => _difficulty;
        public Selectable Selectable => _toggle;

        [SerializeField]
        Image _bg;

        private void Awake()
        {
            _notSelectedSprite = _image.sprite;
            _notSelectedColor = new Color(1, 1, 1, 0);
        }
        public void OnChecked(bool on)
        {
            if (on)
            {
                _parent.SetCheckedToggle(this);
                _image.sprite = _selectedSprite;
            }
            else _image.sprite = _notSelectedSprite;
        }
        public void SwitchOn(bool on)
        {
            _toggle.isOn = on;
        }
        public void OnSelect(BaseEventData eventData)
        {
            _bg.color = _selectedColor;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _bg.color = _notSelectedColor;
        }
    }
}
