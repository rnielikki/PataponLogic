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

            Character.CharacterEnvironment.AnimalSightMultiplier = 0.6f;
        }

        public void OnWeatherStopped(WeatherType newType)
        {
            WeatherInfo.Current.FireRateMultiplier = 1;
            WeatherInfo.Current.StopWeatherSound();
            Character.CharacterEnvironment.AnimalSightMultiplier = 1;

            gameObject.SetActive(false);
        }
    }
}
