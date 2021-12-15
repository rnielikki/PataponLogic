using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Map
{
    public class MapDataContainer : ISerializationCallbackReceiver
    {
        private MapData _mapData;
        public MapData MapData => _mapData;
        [SerializeField]
        private int _mapDataIndex;
        public int Index => _mapDataIndex;

        [SerializeField]
        private Weather.WeatherType _currentWeather;
        public Weather.WeatherType CurrentWeather => _currentWeather;
        [SerializeField]
        private Weather.WindType _currentWind;
        public Weather.WindType CurrentWind => _currentWind;

        private Dictionary<Weather.WeatherType, float> _weatherValueMap = new Dictionary<Weather.WeatherType, float>();

        [SerializeField]
        private int _reachedMaxLevel; //for example difficulty hard->easy->hard scenario

        [SerializeField]
        private int _level;
        public int Level => _level;

        public const string MapPath = "Map/Levels/";

        internal MapDataContainer(int index)
        {
            _mapDataIndex = index;
            _mapData = LoadResource();
            _reachedMaxLevel = _level = 1;
        }
        public string GetNameWithLevel()
        {
            if (_level > 1)
            {
                var str = $"{MapData.Name} Lv. {_level}";
                if (_level >= MapData.GetMaxLevel())
                {
                    str += "★";
                }
                return str;
            }
            else
            {
                return MapData.Name;
            }
        }
        internal void ChangeWeather()
        {
            var rand = Random.Range(0, 1f);
            _currentWind = (rand < MapData.NoWindChance) ? Weather.WindType.None : Weather.WindType.Changing;

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

        internal void LevelUp()
        {
            if (Level < MapData.GetMaxLevel())
            {
                _level++;
                if (_reachedMaxLevel < _level) _reachedMaxLevel = _level;
            }
        }
        internal bool CanLoadNextLevel() => Level >= MapData.LevelRequirementForNext;
        private MapData LoadResource()
        {
            var mapData = Resources.Load<MapData>(MapPath + _mapDataIndex.ToString());
            if (mapData == null) return null;
            mapData.Index = _mapDataIndex;

            LoadWeatherMap(mapData);
            return mapData;
        }
        private void LoadWeatherMap(MapData mapData)
        {
            _weatherValueMap = new Dictionary<Weather.WeatherType, float>()
            {
                { Weather.WeatherType.Rain, mapData.RainWeatherChance },
                { Weather.WeatherType.Storm, mapData.StormWeatherChance },
                { Weather.WeatherType.Fog, mapData.FogWeatherChance },
                { Weather.WeatherType.Snow, mapData.SnowWeatherChance }
            };
        }
        public void OnBeforeSerialize()
        {
            _mapDataIndex = _mapData.Index;
        }
        public void OnAfterDeserialize()
        {
            _mapData = LoadResource();
            var maxLevel = _mapData.GetMaxLevel();
            if (maxLevel < _level)
            {
                _level = maxLevel;
            }
        }
    }
}
