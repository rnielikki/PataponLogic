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
        private UnityEngine.Events.UnityAction _callback;
        private float _speed;
        private static GameObject _fadingResource { get; set; }
        public static void Create(bool fadingIn, float speed = 2, UnityEngine.Events.UnityAction callback = null)
        {
            if (_fadingResource == null)
            {
                _fadingResource = Resources.Load<GameObject>("Common/Display/FadeScreen");
            }

            Core.Global.GlobalData.EnableAllInputs();
            Instantiate(_fadingResource)
                .GetComponent<ScreenFading>()
                .Set(fadingIn, speed, callback);
        }
        public void Set(bool fadingIn, float speed = 1, UnityEngine.Events.UnityAction callback = null)
        {
            DontDestroyOnLoad(gameObject);
            _image = GetComponentInChildren<Image>();
            _direction = fadingIn ? -1 : 1;
            _activated = true;
            _callback = callback;
            _speed = speed;
            if (fadingIn)
            {
                _image.color = new Color(0, 0, 0, 1);
            }
            else
            {
                _image.color = new Color(0, 0, 0, 0);
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (_activated)
            {
                var clr = _image.color;
                clr.a = Mathf.Clamp01(clr.a + _speed * _direction * Time.deltaTime);
                _image.color = clr;
                if ((clr.a == 0 && _direction < 0) || (clr.a == 1 && _direction > 0))
                {
                    _callback?.Invoke();
                    _activated = false;
                    if (_direction < 0) Destroy(gameObject);
                    else SceneManager.sceneUnloaded += DestroyThis;
                }
            }
        }
        void DestroyThis(Scene scene)
        {
            Core.Global.GlobalData.EnableAllInputs();
            SceneManager.sceneUnloaded -= DestroyThis;
            Destroy(gameObject);
        }
    }
}
