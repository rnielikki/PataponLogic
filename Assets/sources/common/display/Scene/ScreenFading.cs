using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PataRoad.Common.GameDisplay
{

    public class ScreenFading : MonoBehaviour
    {
        private Image _image;
        private bool _activated;
        private int _direction;
        private float _speed;
        private static ScreenFading _current { get; set; }

        private ScreenFadingType _fadingType;
        private string _newSceneName;
        [SerializeField]
        private GameObject _loadingImage;
        private bool _usingLoadingImage;

        private void Start()
        {
            _current = this;
            _image = GetComponentInChildren<Image>();
            gameObject.SetActive(false);
        }
        /// <summary>
        /// Creates a smooth scene change.
        /// </summary>
        /// <param name="fadingType">determines if it's fade in/out/both.</param>
        /// <param name="speed">Speed to change. Default is 2,5. if the speed is less than 1, It turns to default value.</param>
        /// <param name="color">Color for fading. Usually it's black.</param>
        /// <param name="sceneToChange">Scene to change smoothly.</param>
        /// <param name="useLoadingImage">Use Loading image. Activated only once. Doesn't work with fading out.</param>
        public static void Create(
            ScreenFadingType fadingType, float speed, Color color, string sceneToChange,
            bool useLoadingImage = false)
        {
            if (_current._activated && _current._direction > 0) return;
            if (speed < 1) speed = 2.5f;
            Core.Global.GlobalData.GlobalInputActions.DisableAllInputs();
            _current.Set(fadingType, speed, color, sceneToChange, useLoadingImage);
        }
        private void Set(
            ScreenFadingType fadingType, float speed, Color color,
            string sceneToChange, bool useLoadingImage)
        {
            _fadingType = fadingType;
            var fadingIn = _fadingType == ScreenFadingType.FadeIn;
            _direction = fadingIn ? -1 : 1;
            _current._loadingImage.SetActive(_fadingType != ScreenFadingType.FadeOut);
            _loadingImage.SetActive(false);
            _usingLoadingImage = useLoadingImage;

            _speed = speed;
            _newSceneName = sceneToChange;
            if (fadingIn)
            {
                SceneManager.sceneLoaded += ActivateFadeIn;
                _loadingImage.SetActive(useLoadingImage);
                _image.color = new Color(color.r, color.g, color.b, 1);
            }
            else
            {
                _image.color = new Color(color.r, color.g, color.b, 0);
                _loadingImage.SetActive(false);
                _activated = true;
            }
            gameObject.SetActive(true);
        }
        private void ActivateFadeIn(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= ActivateFadeIn;
            if (_usingLoadingImage) _loadingImage.SetActive(false);
            _activated = true;
        }
        void Update()
        {
            if (_activated)
            {
                var clr = _image.color;
                clr.a = Mathf.Clamp01(clr.a + (_speed * _direction * Time.deltaTime));
                _image.color = clr;

                if (clr.a == 0 && _direction < 0) //fading in ready
                {
                    End();
                }
                else if (clr.a == 1 && _direction > 0) //fading out ready
                {
                    if (_usingLoadingImage)
                    {
                        _loadingImage.SetActive(true);
                    }
                    if (_fadingType == ScreenFadingType.FadeOut)
                    {
                        SceneManager.sceneLoaded += DestroyThis;
                    }
                    else
                    {
                        SceneManager.sceneLoaded += ChangeToFadingIn;
                    }
                    _activated = false;
                    SceneManager.LoadScene(_newSceneName);
                }
            }
        }
        private void ChangeToFadingIn(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= ChangeToFadingIn;
            _usingLoadingImage = false;
            _loadingImage.SetActive(false);
            _direction = -1;
            _activated = true;
        }

        private void End()
        {
            SceneManager.sceneLoaded -= DestroyThis;
            SceneManager.sceneLoaded -= ChangeToFadingIn;
            SceneManager.sceneLoaded -= ActivateFadeIn;
            Core.Global.GlobalData.GlobalInputActions.EnableAllInputs();
            _activated = false;
            _usingLoadingImage = false;
            _loadingImage.SetActive(false);
            gameObject.SetActive(false);
        }
        void DestroyThis(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= DestroyThis;
            End();
        }
        private void OnDestroy()
        {
            End();
            _current = null;
        }
    }
}
