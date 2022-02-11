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
        private bool _cleared;
        public bool Cleared
        {
            get => _cleared;
            internal set => _cleared = value;
        }

        private int _level;
        public int Level => _level;

        public string Description => Cleared ? _mapData.DescriptionAfterClear : _mapData.DescriptionBeforeClear;

        public const string MapPath = "Map/Levels/";

        private MapDataContainer()
        {
        }
        internal static MapDataContainer Create(int index)
        {
            var mapData = LoadResource(index);
            if (mapData == null) return null;

            var mapDataContainer = new MapDataContainer();
            mapDataContainer._mapData = mapData;
            mapDataContainer._mapDataIndex = index;
            mapDataContainer._reachedMaxLevel = mapDataContainer._level = 1;
            mapDataContainer._weather = new MapWeather(mapData);
            return mapDataContainer;
        }
        public string GetNameWithLevel()
        {
            if (_level > 1)
            {
                var str = $"{MapData.Name} Lv. {_level}";
                if (_level >= MapData.GetMaxLevel())
                {
                    str += " *";
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
            if (MapData.HasLevel && Level < MapData.GetMaxLevel())
            {
                _level++;
                if (_reachedMaxLevel < _level) _reachedMaxLevel = _level;
            }
        }
        internal bool CanLoadNextLevel() => !MapData.HasLevel || Level >= MapData.LevelRequirementForNext;
        private static MapData LoadResource(int index)
        {
            var mapData = Resources.Load<MapData>(MapPath + index.ToString());
            if (mapData == null) return null;
            mapData.Index = index;

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
            _mapData = LoadResource(_mapDataIndex);
            _weather.LoadWeatherMap(_mapData);
            _level = Mathf.Min(_mapData.GetMaxLevel(), _reachedMaxLevel);
        }
    }
}
