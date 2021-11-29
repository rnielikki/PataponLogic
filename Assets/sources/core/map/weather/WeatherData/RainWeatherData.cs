using UnityEngine;

namespace PataRoad.Core.Map.Weather
{
    class RainWeatherData : MonoBehaviour, IWeatherData
    {
        public WeatherType Type => WeatherType.Rain;
        [SerializeField]
        public AudioClip _sound;

        public void OnWeatherStarted()
        {
            if (gameObject.activeSelf) return;
            gameObject.SetActive(true);
            WeatherInfo.Current.SetWeatherSound(_sound);
            WeatherInfo.Current.FireRateMultiplier = 0.5f;
        }

        public void OnWeatherStopped(WeatherType newType)
        {
            WeatherInfo.Current.FireRateMultiplier = 1;
            WeatherInfo.Current.StopWeatherSound();
            gameObject.SetActive(false);
        }
    }
}
