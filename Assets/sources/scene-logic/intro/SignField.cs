using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.Intro
{
    class SignField : MonoBehaviour
    {
        private RectTransform _rectTransform;
        [SerializeField]
        Image _image;
        [SerializeField]
        Color _selectedColor;
        Color _notSelectedColor;
        public RectTransform RectTransform => _rectTransform;
        public bool IsOn { get; private set; }
        [SerializeField]
        AudioClip _soundWhenOn;
        [SerializeField]
        AudioClip _soundWhenOff;
        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _notSelectedColor = _image.color;
        }
        internal void MarkAsSelected()
        {
            if (!IsOn)
            {
                Core.Global.GlobalData.Sound.PlayInScene(_soundWhenOn);
                _image.color = _selectedColor;
                IsOn = true;
            }
        }
        internal void MarkAsNotSelected()
        {
            if (IsOn)
            {
                Core.Global.GlobalData.Sound.PlayInScene(_soundWhenOff);
                _image.color = _notSelectedColor;
                IsOn = false;
            }
        }
    }
}
