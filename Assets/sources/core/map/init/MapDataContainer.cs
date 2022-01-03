using UnityEngine;

namespace PataRoad.Core.Map
{
    [System.Serializable]
    public class MapDataContainer : ISerializationCallbackReceiver
    {
        private MapData _mapData;
        public MapData MapData => _mapData;
        [SerializeField]
        private int _mapDataIndex;
        public int Index => _mapDataIndex;

        [SerializeReference]
        private MapWeather _weather;
        public MapWeather Weather => _weather;

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
            _weather = new MapWeather(_mapData);
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
        internal void ChangeWeather() => _weather.ChangeWeather();

        internal void LevelUp()
        {
            if (Level < MapData.GetMaxLevel())
            {
                _level++;
                if (_reachedMaxLevel < _level) _reachedMaxLevel = _level;
            }
        }
        internal bool CanLoadNextLevel() => MapData.HasLevel && Level >= MapData.LevelRequirementForNext;
        private MapData LoadResource()
        {
            var mapData = Resources.Load<MapData>(MapPath + _mapDataIndex.ToString());
            if (mapData == null) return null;
            mapData.Index = _mapDataIndex;

            return mapData;
        }
        public void OnBeforeSerialize()
        {
            _mapDataIndex = _mapData.Index;
        }
        public void OnAfterDeserialize()
        {
            //Hmm...
        }
        public void LoadDataAfterDeserialization()
        {
            _mapData = LoadResource();
            _weather.LoadWeatherMap(_mapData);
            var maxLevel = _mapData.GetMaxLevel();
            if (maxLevel < _level)
            {
                _level = maxLevel;
            }
        }
    }
}
