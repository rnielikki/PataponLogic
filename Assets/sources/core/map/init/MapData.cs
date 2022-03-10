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
        [Header("Basic Informations")]
        [SerializeField]
        private string _name;
        [SerializeField]
        [Tooltip("Optional. If empty, it should use the default name.")]
        private string _nameAfterClear;
        public string Name => _name;
        public string NameAfterClear => string.IsNullOrWhiteSpace(_nameAfterClear) ? _name : _nameAfterClear;
        [SerializeField]
        [TextArea]
        private string _descriptionBeforeClear;
        public string DescriptionBeforeClear => _descriptionBeforeClear;
        [SerializeField]
        [TextArea]
        private string _descriptionAfterClear;
        public string DescriptionAfterClear => _descriptionAfterClear;
        [SerializeField]
        private MapType _type;
        public MapType Type => _type;
        [SerializeField]
        bool _openOnlyOnce;
        public bool OpenOnlyOnce => _openOnlyOnce;
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
        private float _rainWeatherChance;
        public float RainWeatherChance => _rainWeatherChance;
        [SerializeField]
        private float _stormWeatherChance;
        public float StormWeatherChance => _stormWeatherChance;
        [SerializeField]
        private float _snowWeatherChance;
        public float SnowWeatherChance => _snowWeatherChance;
        [SerializeField]
        private float _fogWeatherChance;
        public float FogWeatherChance => _fogWeatherChance;

        [SerializeField]
        private float _noWindChance;
        public float NoWindChance => _noWindChance;

        [Header("Mission")]
        [SerializeField]
        private bool _useMissionTower;
        public virtual bool UseMissionTower => _useMissionTower;
        [SerializeField]
        private bool _filledMissionCondition;
        public bool FilledMissionCondition => _filledMissionCondition;
        [SerializeField]
        private int _tipIndexOnFail;
        public int TipIndexOnFail => _tipIndexOnFail;
        [SerializeField]
        private int _tipIndexOnSuccess;
        public int TipIndexOnSuccess => _tipIndexOnSuccess;

        [Header("Level Requirements")]
        [SerializeField]
        bool _hasLevel = true;
        public bool HasLevel => _hasLevel;
        [SerializeField]
        int _levelRequiprementForNext = 2;
        public int LevelRequirementForNext => _levelRequiprementForNext;
        [SerializeField]
        int _maxLevelOnEasy;
        [SerializeField]
        int _maxLevelOnNormal;
        [SerializeField]
        int _maxLevelOnHard;

        /// <summary>
        /// Gets real maximum level, it's equal as difficulty hard max level.
        /// </summary>
        public int AbsoluteMaxLevel => _maxLevelOnHard;

        [Header("Story")]
        [SerializeField]
        private bool _loadStoryOnlyOnce;
        public bool LoadStoryOnlyOnce => _loadStoryOnlyOnce;
        [SerializeField]
        string _nextStoryNameOnSuccess;
        public string NextStoryOnSuccess =>
            string.IsNullOrWhiteSpace(_nextStoryNameOnSuccess) ? null : _nextStoryNameOnSuccess;
        [SerializeField]
        string _nextStoryNameOnFail;
        public string NextStoryOnFail =>
            string.IsNullOrWhiteSpace(_nextStoryNameOnFail) ? null : _nextStoryNameOnFail;

        public int GetMaxLevel()
        {
            if (_hasLevel) return CheckMaxLevel();
            else return 1;
        }
        private int CheckMaxLevel() =>
            Rhythm.RhythmEnvironment.Difficulty switch
            {
                Rhythm.Difficulty.Easy => _maxLevelOnEasy,
                Rhythm.Difficulty.Normal => _maxLevelOnNormal,
                Rhythm.Difficulty.Hard => _maxLevelOnHard,
                _ => throw new System.ArgumentException("???")
            };

        private void OnValidate()
        {
            if (RainWeatherChance + StormWeatherChance + FogWeatherChance + SnowWeatherChance > 1)
            {
                var chance = RainWeatherChance + StormWeatherChance + FogWeatherChance + SnowWeatherChance;
                Debug.LogWarning($"Weather sum is [{chance}], more than 1," +
                    $"the weather probability won't work correctly.\n"
                    + "The probability of each weather must be range of [0-1] and their sum must be 1 or less.");
            }
            if (_noWindChance > 1 || _noWindChance < 0)
            {
                Debug.LogWarning("No wind chance must be range of [0-1], otherwise it won't work correctly");
            }
        }
    }
}
