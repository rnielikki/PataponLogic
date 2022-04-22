using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.Story.Actions
{
    class StoryImage : MonoBehaviour
    {
        [SerializeField]
        private Image _image;
        private AspectRatioFitter _imageFitter;
        [SerializeField]
        private float _fadeSpeed;
        [SerializeField]
        private bool _preloaded;
        private bool _fading;
        private bool _fadingIn;

        private bool _fadingBlack;
        private bool _fadingBlackIn; //avoid confliction

        private Sprite _imageToReplace;
        void Start()
        {
            if (!_preloaded)
            {
                var clr = _image.color;
                clr.a = 0;
                _image.color = clr;
                _image.enabled = false;
            }
            _imageFitter = _image.GetComponent<AspectRatioFitter>();
        }
        public void LoadImage(Sprite image)
        {
            if (!_image.enabled)
            {
                _image.sprite = image;
                _image.preserveAspect = true; //need to do manually when image is loaded
                _image.enabled = true;
                Core.Global.GlobalData.GlobalInputActions.DisableOkCancelInputs();
                _fading = true;
                _fadingIn = true;
            }
            else //change Image
            {
                _imageToReplace = image;
                _fadingBlack = true;
                _fadingBlackIn = false;
            }
            if (image != null) _imageFitter.aspectRatio = image.rect.width / image.rect.height;
        }
        public void LoadAsColor() => LoadImage(null);
        public void UnloadImage()
        {
            Core.Global.GlobalData.GlobalInputActions.DisableOkCancelInputs();
            _fading = true;
            _fadingIn = false;
        }
        private void Update()
        {
            if (_fading)
            {
                var color = _image.color;
                if (_fadingIn)
                {
                    color.a = Mathf.Min(1, color.a + (_fadeSpeed * Time.deltaTime));
                    if (color.a == 1)
                    {
                        _fading = false;
                        Core.Global.GlobalData.GlobalInputActions.EnableOkCancelInputs();
                    }
                }
                else
                {
                    color.a = Mathf.Max(0, color.a - (_fadeSpeed * Time.deltaTime));
                    if (color.a == 0)
                    {
                        _fading = false;
                        _image.enabled = false;
                        Core.Global.GlobalData.GlobalInputActions.EnableOkCancelInputs();
                    }
                }
                _image.color = color;
            }
            else if (_fadingBlack)
            {
                var color = _image.color;
                float colorValue;
                if (_fadingBlackIn)
                {
                    colorValue = Mathf.Min(1, color.r + (_fadeSpeed * Time.deltaTime));
                    if (colorValue == 1)
                    {
                        _fadingBlack = false;
                    }
                }
                else
                {
                    colorValue = Mathf.Max(0, color.r - (_fadeSpeed * Time.deltaTime));
                    if (colorValue == 0)
                    {
                        _fadingBlackIn = true;
                        _image.sprite = _imageToReplace;
                    }
                }
                _image.color = new Color(colorValue, colorValue, colorValue, 1);
            }
        }
    }
}
