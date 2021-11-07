using UnityEngine;

namespace PataRoad.Core.Map.Weather
{
    public class Wind : MonoBehaviour
    {
        private WindZone _zone;
        private const float _windRange = 3; //min is -_windRange and max is _windRange
        /// <summary>
        /// 0-1 range of wind magnitude. 0 is maximum headwind, 1 is maximum tailwind.
        /// </summary>
        public float Magnitude => Mathf.InverseLerp(-_windRange, _windRange, _zone.windMain);
        // Start is called before the first frame update
        void Awake()
        {
            _zone = GetComponent<WindZone>();
        }
    }
}
