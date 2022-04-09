using UnityEngine;

namespace PataRoad.Core.Map.Weather
{
    class SnowWeatherData : MonoBehaviour, IWeatherData
    {
        public WeatherType Type => WeatherType.Snow;
        [SerializeField]
        private GameObject _darkBg;

        public void OnWeatherStarted(bool firstInit)
        {
            gameObject.SetActive(true);
            WeatherInfo.Current.StopWeatherSound();
            WeatherInfo.Current.FireRateMultiplier = 0.25f;
            WeatherInfo.Current.IceRateMultiplier = 1.5f;
            if (_darkBg != null) _darkBg.SetActive(true);

            WeatherInfo.Current.Clouds.Show();
        }

        public void OnWeatherStopped(WeatherType newType)
        {
            WeatherInfo.Current.FireRateMultiplier = 1;
            WeatherInfo.Current.IceRateMultiplier = 1;
            if (_darkBg != null) _darkBg.SetActive(false);
            gameObject.SetActive(false);

            WeatherInfo.Current.Clouds.StartHiding();
        }
        private void OnParticleCollision(GameObject other)
        {
            other.GetComponent<IWeatherReceiver>()?.ReceiveWeather(WeatherType.Snow);
        }
    }
}
