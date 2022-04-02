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

        private void Start()
        {
            _current = this;
            _image = GetComponentInChildren<Image>();
            gameObject.SetActive(false);
        }
        public static void Create(
            ScreenFadingType fadingType, float speed, Color color, string sceneToChange)
        {
            if (_current._activated) return;
            if (speed < 1) speed = 2;
            Core.Global.GlobalData.GlobalInputActions.DisableAllInputs();
            _current.Set(fadingType, speed, color, sceneToChange);
        }
        private void Set(
            ScreenFadingType fadingType, float speed, Color color,
            string sceneToChange)
        {
            _fadingType = fadingType;
            var fadingIn = _fadingType == ScreenFadingType.FadeIn;
            _direction = fadingIn ? -1 : 1;
            _activated = true;
            _speed = speed;
            _newSceneName = sceneToChange;
            if (fadingIn)
            {
                _image.color = new Color(color.r, color.g, color.b, 1);
            }
            else
            {
                _image.color = new Color(color.r, color.g, color.b, 0);
            }
            gameObject.SetActive(true);
        }
        // Update is called once per frame
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
                    if (_fadingType == ScreenFadingType.FadeOut)
                    {
                        SceneManager.sceneUnloaded += DestroyThis;
                    }
                    else
                    {
                        SceneManager.sceneUnloaded += ChangeToFadingIn;
                    }
                    _activated = false;
                    SceneManager.LoadScene(_newSceneName);
                }
            }
        }
        private void ChangeToFadingIn(Scene scene)
        {
            SceneManager.sceneUnloaded -= ChangeToFadingIn;
            _direction = -1;
            _activated = true;
        }
        private void End()
        {
            Core.Global.GlobalData.GlobalInputActions.EnableAllInputs();
            _activated = false;
            gameObject.SetActive(false);
        }
        void DestroyThis(Scene scene)
        {
            SceneManager.sceneUnloaded -= DestroyThis;
            End();
        }
        private void OnDestroy()
        {
            _current = null;
        }
    }
}
