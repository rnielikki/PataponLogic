using UnityEngine;

namespace PataRoad.Core.Map
{
    [CreateAssetMenu(fileName = "MapData", menuName = "MapData")]
    public class MapData : ScriptableObject
    {
        public int Index { get; set; }
        [SerializeField]
        private int _nextIndex;
        public int NextIndex => _nextIndex;
        [SerializeField]
        private string _name;
        public string Name => _name;
        [Header("In-game environment")]
        [SerializeField]
        private string _defaultMusic;
        public string DefaultMusic => _defaultMusic;
        [SerializeField]
        private string _background;
        public string BackgroundName => _background;
        [Header("In-game data")]
        [SerializeField]
        private int _maxBossSummonCount;
        public int SummonCount => _maxBossSummonCount;
        [Header("Weather")]
        [SerializeField]
        private bool _randomWeather;
        public bool RandomWeather => _randomWeather;
        [SerializeField]
        Weather.WeatherType _weather;
        public Weather.WeatherType Weather => _weather;
        [SerializeField]
        private bool _randomWind;
        public bool RandomWind => _randomWind;
        [SerializeField]
        Weather.WindType _windType;
        public Weather.WindType WindType => _windType;
        [Header("Mission")]
        [SerializeField]
        private bool _useMissionTower;
        public bool UseMissionTower => _useMissionTower;
        [SerializeField]
        private bool _filledMissionCondition;
        public bool FilledMissionCondition => _filledMissionCondition;
        [SerializeField]
        private int _tipIndexOnFail;
        public int TipIndexOnFail => _tipIndexOnFail;
        [SerializeField]
        private int _tipIndexOnSuccess;
        public int TipIndexOnSuccess => _tipIndexOnSuccess;
    }
}
