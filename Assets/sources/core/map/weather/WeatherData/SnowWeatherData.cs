using UnityEngine;

namespace PataRoad.Core.Map.Weather
{
    class SnowWeatherData : MonoBehaviour, IWeatherData
    {
        public WeatherType Type => WeatherType.Snow;

        public void OnWeatherStarted(bool firstInit)
        {
            gameObject.SetActive(true);
            WeatherInfo.Current.StopWeatherSound();
            WeatherInfo.Current.FireRateMultiplier = 0.25f;
            WeatherInfo.Current.IceRateMultiplier = 1.5f;
        }

        public void OnWeatherStopped(WeatherType newType)
        {
            WeatherInfo.Current.FireRateMultiplier = 1;
            WeatherInfo.Current.IceRateMultiplier = 1;
            gameObject.SetActive(false);
        }
        private void OnParticleCollision(GameObject other)
        {
            other.GetComponent<IWeatherReceiver>()?.ReceiveWeather(WeatherType.Snow);
        }
    }
}
