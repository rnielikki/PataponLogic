using UnityEngine;

namespace PataRoad.Story
{
    [CreateAssetMenu(fileName = "story-data", menuName = "Story Data")]
    public class StoryData : ScriptableObject
    {
        [Header("Weather info")]
        [SerializeField]
        private Core.Map.Weather.WeatherType _weather;
        [SerializeField]
        private bool _randomWeather;
        public Core.Map.Weather.WeatherType Weather => _randomWeather ? GetRandomWeather() : _weather;
        [SerializeField]
        private bool _randomWind;
        [SerializeField]
        private Core.Map.Weather.WindType _windType;
        public Core.Map.Weather.WindType Wind => _randomWind ? GetRandomWind() : _windType;
        [Header("Scene Info")]
        [SerializeField]
        private bool _usePatapolis;
        public bool UsePatapolis => _usePatapolis;
        public string SceneName => _usePatapolis ? "Patapolis" : "StoryScene";
        [SerializeField]
        [Tooltip("If null, goes to the Patapolis")]
        private Core.Map.MapData _nextMap;
        public Core.Map.MapData NextMap => _nextMap;
        [Header("Map info")]
        [SerializeField]
        [Tooltip("This do nothing on Patapolis")]
        private string _background;
        public string Background => _background;
        [SerializeField]
        private AudioClip _music;
        public AudioClip Music => _music;
        [SerializeField]
        [Tooltip("This works only on Patapolis")]
        private float _xCameraPosition;
        public float XCameraPosition => _xCameraPosition;
        [Header("Story Data")]
#pragma warning disable S1104 // Fields should not have public accessibility - we need AGAIN this for serialized array in inspector.
        public StoryAction[] StoryActions;
#pragma warning restore S1104 // Fields should not have public accessibility
        [SerializeField]
        private GameObject _resource;
        public GameObject Resource => _resource;

        private Core.Map.Weather.WeatherType GetRandomWeather()
        {
            var weathers = System.Enum.GetValues(typeof(Core.Map.Weather.WeatherType));
            return (Core.Map.Weather.WeatherType)Random.Range(0, weathers.Length);
        }
        private Core.Map.Weather.WindType GetRandomWind()
        {
            var winds = System.Enum.GetValues(typeof(Core.Map.Weather.WindType));
            return (Core.Map.Weather.WindType)Random.Range(0, winds.Length);
        }
    }
}
