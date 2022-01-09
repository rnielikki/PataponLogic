using UnityEngine;

namespace PataRoad.Core.Map.Weather
{
    public class WeatherInfo : MonoBehaviour
    {
        private WeatherType _defaultWeather;

        [SerializeField]
        private Wind _wind;
        public Wind Wind => _wind;

        private IWeatherData _currentWeather;

        public static WeatherInfo Current { get; private set; }

        private System.Collections.Generic.Dictionary<WeatherType, IWeatherData> _weatherTypeDataMap;
        public float FireRateMultiplier { get; set; } = 1;
        public float IceRateMultiplier { get; set; } = 1;

        public AudioSource AudioSource { get; private set; }
        // Start is called before the first frame update
        private bool _wasPlaying;
        public void Init(WeatherType type)
        {
            _weatherTypeDataMap = new System.Collections.Generic.Dictionary<WeatherType, IWeatherData>()
            {
                { WeatherType.Clear, null }
            };
            foreach (var weather in GetComponentsInChildren<IWeatherData>(true))
            {
                if (_weatherTypeDataMap.ContainsKey(weather.Type))
                {
                    throw new System.InvalidOperationException($"The weather type {weather.Type} already exists!");
                }
                _weatherTypeDataMap.Add(weather.Type, weather);
            }

            Current = this;
            AudioSource = GetComponent<AudioSource>();
            _defaultWeather = type;
            ChangeWeather(_defaultWeather);
        }
        public void ChangeWeather(WeatherType type)
        {
            var weatherType = _currentWeather?.Type ?? WeatherType.Clear;
            if (weatherType == type) return;
            if (weatherType != WeatherType.Clear)
            {
                EndChangingWeather(type);
            }
            _currentWeather = _weatherTypeDataMap[type];
            _currentWeather?.OnWeatherStarted();
        }
        public void EndChangingWeather(WeatherType type)
        {
            _currentWeather?.OnWeatherStopped(type);
            ChangeWeather(_defaultWeather);
        }
        internal void SetWeatherSound(AudioClip clip)
        {
            AudioSource.clip = clip;
            AudioSource.Play();
        }
        internal void StopWeatherSound() => AudioSource.Stop();
        public void PauseWeatherSound()
        {
            _wasPlaying = AudioSource.isPlaying;
            AudioSource.Stop();
        }
        public void ResumeWeatherSound()
        {
            if (_wasPlaying) AudioSource.Play();
        }
        private void OnDestroy()
        {
            Current = null;
        }
    }
}
