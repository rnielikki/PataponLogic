using UnityEngine;

namespace PataRoad.Core.Map.Weather
{
    class FogWeatherData : MonoBehaviour, IWeatherData
    {
        public WeatherType Type => WeatherType.Fog;
        private bool _isSandStorm;
        [SerializeField]
        private UnityEngine.UI.Image _image;
        [SerializeField]
        private Color _sandStormColor;
        [SerializeField]
        private ParticleSystem _sands;

        public void OnWeatherStarted(bool firstInit)
        {
            gameObject.SetActive(true);
            WeatherInfo.Current.StopWeatherSound();
            Character.CharacterEnvironment.Sight = Character.CharacterEnvironment.OriginalSight * 0.8f;
            Character.CharacterEnvironment.AnimalSightMultiplier = 0.8f;
            if (Background.BackgroundLoader.CurrentTheme == Background.BackgroundLoader.DesertName)
            {
                _isSandStorm = true;
                WeatherInfo.Current.Wind.StartWind(WindType.Changing);
                _sandStormColor.a = _image.color.a;
                _image.color = _sandStormColor;
                _sands.Play();
            }
            else
            {
                _image.color = new Color(1, 1, 1, _image.color.a);
            }
        }

        public void OnWeatherStopped(WeatherType newType)
        {
            Character.CharacterEnvironment.Sight = Character.CharacterEnvironment.OriginalSight;
            Character.CharacterEnvironment.AnimalSightMultiplier = 1;
            gameObject.SetActive(false);
            if (_isSandStorm)
            {
                _isSandStorm = false;
                WeatherInfo.Current.Wind.StopWind(WindType.Changing);
            }
        }
    }
}
