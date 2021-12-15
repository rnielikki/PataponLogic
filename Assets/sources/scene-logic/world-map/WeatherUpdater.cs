using PataRoad.Core.Map.Weather;
using UnityEngine;

namespace PataRoad.SceneLogic.WorldMap
{
    internal class WeatherUpdater : MonoBehaviour
    {
        [SerializeField]
        UnityEngine.UI.Image _cloudImage;
        [SerializeField]
        UnityEngine.UI.Image _rainImage;
        [SerializeField]
        UnityEngine.UI.Image _snowImage;
        [SerializeField]
        UnityEngine.UI.Image _stormImage;
        [SerializeField]
        UnityEngine.UI.Image _fogImage;
        [SerializeField]
        UnityEngine.UI.Image _windImage;

        private WeatherType _weatherType;
        private bool _hasWind;
        private bool _loaded;

        public void UpdateWeather(WeatherType weatherType, bool hasWind)
        {
            _weatherType = weatherType;
            _hasWind = hasWind;
            if (weatherType == WeatherType.Rain || weatherType == WeatherType.Snow || weatherType == WeatherType.Storm)
            {
                _cloudImage.gameObject.SetActive(true);
                switch (weatherType)
                {
                    case WeatherType.Rain:
                        _rainImage.gameObject.SetActive(true);
                        break;
                    case WeatherType.Snow:
                        _snowImage.gameObject.SetActive(true);
                        break;
                    case WeatherType.Storm:
                        _rainImage.gameObject.SetActive(true);
                        _stormImage.gameObject.SetActive(true);
                        break;
                }
            }
            else if (weatherType == WeatherType.Fog)
            {
                _fogImage.gameObject.SetActive(true);
            }
            if (hasWind)
            {
                _windImage.gameObject.SetActive(true);
            }
            _loaded = true;
        }
        private void OnEnable()
        {
            if (_loaded)
            {
                UpdateWeather(_weatherType, _hasWind);
            }
        }
        private void OnDisable()
        {
            _cloudImage.gameObject.SetActive(false);
            _snowImage.gameObject.SetActive(false);
            _rainImage.gameObject.SetActive(false);
            _stormImage.gameObject.SetActive(false);
            _windImage.gameObject.SetActive(false);
        }
    }
}
