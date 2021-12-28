using PataRoad.Core.Map.Weather;
using UnityEngine;

namespace PataRoad.Story
{
    class StorySceneInfo : MonoBehaviour
    {
        [SerializeField]
        WeatherInfo _weatherInfo;
        internal WeatherInfo Weather => _weatherInfo;
        [SerializeField]
        Wind _wind;
        internal Wind Wind => _wind;
        [SerializeField]
        AudioSource _audioSource;
        internal AudioSource AudioSource => _audioSource;
        [SerializeField]
        Core.Map.Background.BackgroundLoader _background;
        internal Core.Map.Background.BackgroundLoader Background => _background;
        [SerializeField]
        Transform _parent;
        internal Transform Parent => _parent;
    }
}
