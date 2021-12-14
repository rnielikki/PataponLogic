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

        [SerializeReference]
        private List<MapDataContainer> _openMaps = new List<MapDataContainer>();

        [SerializeField]
        private int _lastMapIndex;
        [SerializeField]
        private int _nextMapIndex;
        [SerializeField]
        private bool _succeededLast;
        public bool SuccededLast => _succeededLast;
        public const string MapPath = "Map/Levels/";

        internal MapInfo()
        {
            NextMap = LoadResource(2);
            RefreshAllWeathers();
        }
        public void OpenNext()
        {
            if (NextMap.MapData.NextIndex > 0 && NextMap.CanLoadNextLevel())
            {
                NextMap = LoadResource(NextMap.MapData.NextIndex);
            }
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
            foreach (var map in _openMaps)
            {
                map.ChangeWeather();
            }
        }
        private MapDataContainer LoadResource(int index)
        {
            var map = _openMaps.SingleOrDefault(a => a.Index == index);
            if (map == null)
            {
                map = new MapDataContainer(index);
            }
            if (map.MapData != null)
            {
                _openMaps.Add(map);
            }
            return map;
        }
        public string Serialize()
        {
            _nextMapIndex = NextMap.Index;
            return JsonUtility.ToJson(this);
        }
        public void Deserialize()
        {
            foreach (var map in _openMaps)
            {
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
                NextMap = _openMaps.Last();
            }
            RefreshAllWeathers();
        }
    }
}
