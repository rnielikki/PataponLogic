﻿using PataRoad.Core.Map;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Global
{
    [System.Serializable]
    public class MapInfo : Slots.IPlayerData
    {
        public MapDataContainer NextMap { get; private set; }
        public MapDataContainer LastMap { get; private set; }

        private Dictionary<int, MapDataContainer> _openMaps = new Dictionary<int, MapDataContainer>();

        [SerializeReference]
        private MapDataContainer[] _openMapsForSerializing;

        [SerializeField]
        private int _lastMapIndex;
        //for once mission succeeded and will never open. No hashset because serialization, also there's not so many so forget the performance
        [SerializeField]
        private List<int> _closedMaps = new List<int>();
        [SerializeField]
        private int _nextMapIndex;
        [SerializeField]
        private bool _succeededLast;
        public bool SuccededLast => _succeededLast;
        public const string MapPath = "Map/Levels/";

        [SerializeReference]
        private MapWeather _patapolisWeather;
        public MapWeather PatapolisWeather => _patapolisWeather;

        internal static MapInfo CreateNew()
        {
            var mapInfo = new MapInfo();
            mapInfo.LoadResource(0);
            mapInfo.LoadResource(1);
            mapInfo.NextMap = mapInfo.LoadResource(2);

            mapInfo._patapolisWeather = new MapWeather(new Dictionary<Map.Weather.WeatherType, float>()
            {
                { Map.Weather.WeatherType.Rain, 0.1f },
                { Map.Weather.WeatherType.Storm, 0.05f },
                { Map.Weather.WeatherType.Fog, 0.05f },
                { Map.Weather.WeatherType.Snow, 0.05f }
            }, 0.9f);
            return mapInfo;
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
            if (NextMap.MapData.OpenOnlyOnce && !_closedMaps.Contains(NextMap.MapData.Index))
            {
                _closedMaps.Add(NextMap.MapData.Index);
                _openMaps.Remove(NextMap.MapData.Index);
            }
            else
            {
                NextMap.LevelUp();
            }
            LastMap = NextMap;
            OpenNext();

            _succeededLast = true;
            GlobalData.CurrentSlot.PataponInfo.CustomMusic = null;
            RefreshAllWeathers();
        }
        public void MissionFailed()
        {
            _succeededLast = false;
            GlobalData.CurrentSlot.PataponInfo.CustomMusic = null;
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
            if (_closedMaps.Contains(index)) return LastMap;
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
        public void Serialize()
        {
            _nextMapIndex = NextMap.Index;
            _openMapsForSerializing = _openMaps.Values.ToArray();
        }
        public void Deserialize()
        {
            foreach (var map in _openMapsForSerializing)
            {
                map.LoadDataAfterDeserialization();
                _openMaps.Add(map.Index, map);
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
