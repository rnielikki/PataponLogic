namespace PataRoad.Core.Map.Weather
{
    class RainWeatherData : UnityEngine.MonoBehaviour, IWeatherData
    {
        public WeatherType Type => WeatherType.Rain;

        public void OnWeatherStarted()
        {
            gameObject.SetActive(true);
            WeatherInfo.Current.FireRateMultiplier = 0.5f;
        }

        public void OnWeatherStopped()
        {
            WeatherInfo.Current.FireRateMultiplier = 1;
            gameObject.SetActive(false);
        }
    }
}
