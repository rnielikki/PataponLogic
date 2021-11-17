using UnityEngine;

namespace PataRoad.Core.Map.Weather
{
    public class WeatherInfo : MonoBehaviour
    {
        public static Wind Wind { get; private set; }
        public WeatherType CurrentWeather { get; private set; }

        [SerializeField]
        public WeatherData Rain;
        [SerializeField]
        public WeatherData Snow;
        [SerializeField]
        public WeatherData Fog;
        // Start is called before the first frame update
        void Awake()
        {
            Wind = GetComponentInChildren<Wind>();
        }
        public static void ChangeWeather(WeatherType type)
        {
        }
    }
}
