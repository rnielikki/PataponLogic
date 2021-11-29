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
            ChangeWeather(_defaultWeather);
        }
        public void ChangeWeather(WeatherType type)
        {
            if ((_currentWeather?.Type ?? WeatherType.Clear) == type) return;
            _currentWeather = _weatherTypeDataMap[type];
            _currentWeather?.OnWeatherStarted();
        }
        public void EndChangingWeather()
        {
            _currentWeather?.OnWeatherStopped();
            ChangeWeather(_defaultWeather);
        }
        private void OnDestroy()
        {
#pragma warning disable S2696 // Instance members should not write to "static" fields
            _self = null;
#pragma warning restore S2696 // Instance members should not write to "static" fields
        }
    }
}
