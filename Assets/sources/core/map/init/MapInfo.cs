using PataRoad.Core.Map;
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

        private Dictionary<int, MapDataContainer> _openMaps;
        [SerializeField]
        private int _progress;
        public int Progress => _progress;

        [SerializeReference]
        private MapDataContainer[] _openMapsForSerializing;

        [SerializeField]
        private int _lastMapIndex;
        //for once mission succeeded and will never open. No hashset because serialization, also there's not so many so forget the performance
        [SerializeField]
        private List<int> _closedMaps;
        [SerializeField]
        private int _nextMapIndex;
        [SerializeField]
        private bool _succeededLast;
        public bool SuccededLast => _succeededLast;
        public const string MapPath = "Map/Levels/";

        [SerializeReference]
        private MapWeather _patapolisWeather;
        public MapWeather PatapolisWeather => _patapolisWeather;

        [SerializeField]
        private string _lastMapName;
        /// <summary>
        /// Useful only for slot meta. Shows last played map, even if it's 'only once mission' and not available anymore.
        /// </summary>
        public string LastMapName => _lastMapName;

        /// <summary>
        /// Should be opened late when called <see cref="OpenInIndex(int)"/>
        /// </summary>
        private MapDataContainer _reservedNextMap;
        /// <summary>
        /// When opens <see cref="_reservedNextMap"/>, set it as <see cref="NextMap"/> or not.
        /// This will affect to the NEXT SELECTION on the map (for small UX adjustment).
        /// </summary>
        private bool _setReservedAsNextMap;

        public MapInfo()
        {
            _openMaps = new Dictionary<int, MapDataContainer>();
            _closedMaps = new List<int>();
        }
        internal static MapInfo CreateNew()
        {
            var mapInfo = new MapInfo();
            mapInfo.LastMap = mapInfo.NextMap = mapInfo.LoadResource(0);
            mapInfo._lastMapName = mapInfo.LastMap.Name;
            mapInfo._patapolisWeather = new MapWeather(new Dictionary<Map.Weather.WeatherType, float>()
            {
                { Map.Weather.WeatherType.Rain, 0.1f },
                { Map.Weather.WeatherType.Storm, 0.05f },
                { Map.Weather.WeatherType.Fog, 0.05f },
                { Map.Weather.WeatherType.Snow, 0.05f }
            }, 0.9f);
            return mapInfo;
        }
        public IEnumerable<MapDataContainer> GetAllAvailableMaps() => _openMaps.Values.OrderBy(map => map.Index);
        /// <summary>
        /// Tells that has the map ever opened, including closed map.
        /// </summary>
        /// <param name="index">the index number of the map.</param>
        /// <returns><c>true</c> if map is opened ever (even if closed now), otherwise <c>false</c></returns>
        public bool HasMapOpenedYet(int index) => _openMaps.ContainsKey(index) || _closedMaps.Contains(index);
        public MapDataContainer GetMapByIndex(int index)
        {
            if (_openMaps.TryGetValue(index, out MapDataContainer map)) return map;
            else return null;
        }
        private void OpenNext()
        {
            if (NextMap.MapData.NextIndex > 0 && NextMap.CanLoadNextLevel())
            {
                NextMap = LoadResource(NextMap.MapData.NextIndex);
                _progress = Mathf.Max(_progress, NextMap.Index);
            }
        }
        public void OpenAndSetAsNext(int index)
        {
            var openedMap = OpenInIndex(index, false);
            if (openedMap != null)
            {
                NextMap = openedMap;
                _reservedNextMap = null;
                _setReservedAsNextMap = false;
            }
        }
        public MapDataContainer OpenInIndex(int index, bool setAsNextMap = true)
        {
            if (!_openMaps.ContainsKey(index) && !_closedMaps.Contains(index))
            {
                _reservedNextMap = LoadResource(index);
                _setReservedAsNextMap = setAsNextMap;
                _progress = Mathf.Max(_progress, index);
                return _reservedNextMap;
            }
            else return null;
        }
        public void Select(MapDataContainer data)
        {
            NextMap = data;
        }
        public void OnMissionSucceeded()
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
            NextMap.Cleared = true;
            LastMap = NextMap;
            _lastMapName = LastMap.Name;
            OpenNext();
            if (_reservedNextMap != null)
            {
                if (_setReservedAsNextMap)
                {
                    NextMap = _reservedNextMap;
                    _setReservedAsNextMap = false;
                }
                _reservedNextMap = null;
            }

            _succeededLast = true;
            GlobalData.CurrentSlot.PataponInfo.CustomMusic = null;
            RefreshAllWeathers();
        }
        public void OnMissionFailed()
        {
            _succeededLast = false;
            LastMap = NextMap;
            _lastMapName = LastMap.Name;
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
                map = MapDataContainer.Create(index);
                if (map != null)
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
        }
    }
}
