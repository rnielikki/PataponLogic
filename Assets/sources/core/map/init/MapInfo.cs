using PataRoad.Core.Map;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Global
{
    [System.Serializable]
    public class MapInfo : IPlayerData
    {
        public MapDataContainer NextMap { get; private set; }
        public MapDataContainer LastMap { get; private set; }

        private Dictionary<int, MapDataContainer> _openMaps = new Dictionary<int, MapDataContainer>();

        [SerializeReference]
        private MapDataContainer[] _openMapsForSerializing;

        [SerializeField]
        private int _lastMapIndex;
        [SerializeField]
        private int _nextMapIndex;
        [SerializeField]
        private bool _succeededLast;
        public bool SuccededLast => _succeededLast;
        public const string MapPath = "Map/Levels/";

        [SerializeReference]
        private MapWeather _patapolisWeather;
        public MapWeather PatapolisWeather => _patapolisWeather;

        internal MapInfo()
        {
            LoadResource(0);
            LoadResource(1);
            NextMap = LoadResource(2);

            _patapolisWeather = new MapWeather(new Dictionary<Map.Weather.WeatherType, float>()
            {
                { Map.Weather.WeatherType.Rain, 0.1f },
                { Map.Weather.WeatherType.Storm, 0.05f },
                { Map.Weather.WeatherType.Fog, 0.05f },
                { Map.Weather.WeatherType.Snow, 0.05f }
            }, 0.9f);
        }
        public IEnumerable<MapDataContainer> GetAllAvailableMaps() => _openMaps.Values;

        public MapDataContainer GetMapByIndex(int index)
        {
            if (_openMaps.TryGetValue(index, out MapDataContainer map)) return map;
            else return null;
        }
        public void OpenNext()
        {
            if (NextMap.MapData.NextIndex > 0 && NextMap.CanLoadNextLevel())
            {
                NextMap = LoadResource(NextMap.MapData.NextIndex);
            }
        }
        public void Select(MapDataContainer data)
        {
            NextMap = data;
        }
        public void MissionSucceeded()
        {
            NextMap.LevelUp();
            LastMap = NextMap;
            OpenNext();

            _succeededLast = true;
            GlobalData.PataponInfo.CustomMusic = null;
            RefreshAllWeathers();
        }
        public void MissionFailed()
        {
            _succeededLast = false;
            GlobalData.PataponInfo.CustomMusic = null;
            RefreshAllWeathers();
        }
        public void RefreshAllWeathers()
        {
            RefreshAllMapWeathers();
            _patapolisWeather.ChangeWeather();
        }
        public void RefreshAllMapWeathers()
        {
            foreach (var map in _openMaps)
            {
                map.Value.ChangeWeather();
            }
        }
        private MapDataContainer LoadResource(int index)
        {
            if (!_openMaps.TryGetValue(index, out MapDataContainer map))
            {
                map = new MapDataContainer(index);
                if (map.MapData != null)
                {
                    _openMaps.Add(index, map);
                }
            }
            return map;
        }
        public string Serialize()
        {
            _nextMapIndex = NextMap.Index;
            _openMapsForSerializing = _openMaps.Values.ToArray();
            return JsonUtility.ToJson(this);
        }
        public void Deserialize()
        {
            foreach (var map in _openMapsForSerializing)
            {
                LoadResource(map.Index);
                if (map.Index == _lastMapIndex)
                {
                    LastMap = map;
                }
                if (map.Index == _nextMapIndex)
                {
                    NextMap = map;
                }
            }
            if (NextMap == null)
            {
                NextMap = _openMapsForSerializing.Last();
            }
            RefreshAllWeathers();
        }
    }
}
