using UnityEngine;

namespace PataRoad.Core.Map.Weather
{
    public class WeatherInfo : MonoBehaviour
    {
        public static Wind Wind { get; private set; }

        [SerializeField]
        private WeatherType _defaultWeather;

        private IWeatherData _currentWeather;

        private static WeatherInfo _self;
        public static WeatherInfo Current => _self;

        private System.Collections.Generic.Dictionary<WeatherType, IWeatherData> _weatherTypeDataMap;
        public float FireRateMultiplier { get; set; } = 1;
        public float IceRateMultiplier { get; set; } = 1;

        public AudioSource AudioSource { get; private set; }
        // Start is called before the first frame update
        void Awake()
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

            Wind = GetComponentInChildren<Wind>();

#pragma warning disable S2696 // Instance members should not write to "static" fields
            _self = this;
#pragma warning restore S2696 // Instance members should not write to "static" fields
            AudioSource = GetComponent<AudioSource>();
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
        private void OnDestroy()
        {
#pragma warning disable S2696 // Instance members should not write to "static" fields
            _self = null;
#pragma warning restore S2696 // Instance members should not write to "static" fields
        }
    }
}
