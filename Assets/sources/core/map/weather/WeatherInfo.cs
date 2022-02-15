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
        public WeatherType CurrentWeather => _currentWeather?.Type ?? WeatherType.Clear;

        public static WeatherInfo Current { get; private set; }

        private System.Collections.Generic.Dictionary<WeatherType, IWeatherData> _weatherTypeDataMap;
        public float FireRateMultiplier { get; set; } = 1;
        public float IceRateMultiplier { get; set; } = 1;
        public UnityEngine.Events.UnityEvent<WeatherType> OnWeatherChanged { get; } = new UnityEngine.Events.UnityEvent<WeatherType>();

        public AudioSource AudioSource { get; private set; }
        private bool _wasPlaying;
        public void Init(WeatherType type, WindType windType)
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
            Wind.Init(windType);
            ChangeWeather(_defaultWeather, true);
        }
        public void ChangeWeather(WeatherType type) => ChangeWeather(type, false);
        private void ChangeWeather(WeatherType type, bool firstInit)
        {
            var weatherType = _currentWeather?.Type ?? WeatherType.Clear;
            if (weatherType == type) return;
            if (!firstInit)
            {
                _currentWeather?.OnWeatherStopped(_defaultWeather);
            }
            _currentWeather = _weatherTypeDataMap[type];
            _currentWeather?.OnWeatherStarted(firstInit);
            OnWeatherChanged.Invoke(type);
        }
        public void EndChangingWeather()
        {
            if (_defaultWeather == (_currentWeather?.Type ?? WeatherType.Clear)) return;
            ChangeWeather(_defaultWeather, false);
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
