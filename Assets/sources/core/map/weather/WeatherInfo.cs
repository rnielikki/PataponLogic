using UnityEngine;

namespace PataRoad.Core.Map.Weather
{
    public class WeatherInfo : MonoBehaviour
    {
        public static Wind Wind { get; private set; }
        // Start is called before the first frame update
        void Awake()
        {
            Wind = GetComponentInChildren<Wind>();
        }
    }
}
