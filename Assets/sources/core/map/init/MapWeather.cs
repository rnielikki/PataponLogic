using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Map
{
    [System.Serializable]

    public class MapWeather : ISerializationCallbackReceiver
    {

        [SerializeField]
        private Weather.WeatherType _currentWeather;
        public Weather.WeatherType CurrentWeather => _currentWeather;
        [SerializeField]
        private Weather.WindType _currentWind;
        public Weather.WindType CurrentWind => _currentWind;

        private Dictionary<Weather.WeatherType, float> _weatherValueMap;
        [System.NonSerialized]
        private float _noWindChance;

        public MapWeather()
        {
            _weatherValueMap = new Dictionary<Weather.WeatherType, float>();
        }

        internal MapWeather(Dictionary<Weather.WeatherType, float> weatherValueMap, float noWindChance)
        {
            _weatherValueMap = weatherValueMap;
            _noWindChance = noWindChance;
            ChangeWeather();
        }
        internal MapWeather(MapData mapData)
        {
            LoadWeatherMap(mapData);
            ChangeWeather();
        }
        internal void ChangeWeather()
        {
            var rand = Random.Range(0, 1f);
            _currentWind = (rand < _noWindChance) ? Weather.WindType.None : Weather.WindType.Changing;

            rand = Random.Range(0, 1f);
            float prob = 0;

            foreach (var kv in _weatherValueMap)
            {
                prob += kv.Value;
                if (rand < prob)
                {
                    _currentWeather = kv.Key;
                    return;
                }
            }
            _currentWeather = Weather.WeatherType.Clear;
        }
        /// <summary>
        /// Call after map data is deserialized.
        /// </summary>
        /// <param name="mapData">Map data.</param>
        internal void LoadWeatherMap(MapData mapData)
        {
            _weatherValueMap = new Dictionary<Weather.WeatherType, float>()
            {
                { Weather.WeatherType.Rain, mapData.RainWeatherChance },
                { Weather.WeatherType.Storm, mapData.StormWeatherChance },
                { Weather.WeatherType.Fog, mapData.FogWeatherChance },
                { Weather.WeatherType.Snow, mapData.SnowWeatherChance }
            };
            _noWindChance = mapData.NoWindChance;
        }

        public void OnAfterDeserialize()
        {
            //nothing to do :)
        }

        public void OnBeforeSerialize()
        {
            //I mean really nothing to do!
        }
    }
}
