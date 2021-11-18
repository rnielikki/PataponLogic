using UnityEngine;

namespace PataRoad.Core.Map.Weather
{
    public class WeatherInfo : MonoBehaviour
    {
        public static Wind Wind { get; private set; }
        public WeatherType CurrentWeather { get; private set; }

        [SerializeField]
        private WeatherType _defaultWeather;

        private WeatherData _currentWeatherData;

        [SerializeField]
        private WeatherData _rain;
        [SerializeField]
        private WeatherData _snow;
        [SerializeField]
        private WeatherData _fog;

        private static WeatherInfo _self;

        private System.Collections.Generic.Dictionary<WeatherType, WeatherData> _weatherTypeDataMap;
        // Start is called before the first frame update
        void Awake()
        {
            _weatherTypeDataMap = new System.Collections.Generic.Dictionary<WeatherType, WeatherData>()
            {
                { WeatherType.Clear, null },
                { WeatherType.Rain, _rain },
                { WeatherType.Snow, _snow },
                { WeatherType.Fog, _fog }
            };
            _defaultWeather = CurrentWeather;
            _currentWeatherData = _weatherTypeDataMap[_defaultWeather];
            Wind = GetComponentInChildren<Wind>();
#pragma warning disable S2696 // Instance members should not write to "static" fields
            _self = this;
#pragma warning restore S2696 // Instance members should not write to "static" fields
        }
        public static void ChangeWeather(WeatherType type)
        {
            if (_self.CurrentWeather == type) return;
            _self.CurrentWeather = type;
            _self._currentWeatherData = _self._weatherTypeDataMap[type];
        }
        public static void EndChangingWeather()
        {
            _self.CurrentWeather = _self._defaultWeather;
            _self._currentWeatherData = _self._weatherTypeDataMap[_self._defaultWeather];
        }
    }
}
