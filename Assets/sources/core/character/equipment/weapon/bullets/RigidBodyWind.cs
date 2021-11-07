using UnityEngine;
using PataRoad.Core.Map.Weather;

namespace PataRoad.Core.Character.Equipment.Weapon
{
    /// <summary>
    /// Set direction of instantiated object.
    /// </summary>
    public class RigidBodyWind : MonoBehaviour
    {
        Rigidbody2D _rigidbody;
        private Wind _wind;
        private static readonly Vector2 _velocityMultiplier = Vector2.right * 0.001f;
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _wind = WeatherInfo.Wind;
        }
        private void FixedUpdate()
        {
            _rigidbody.velocity += _wind.Magnitude * _velocityMultiplier / _rigidbody.mass;
        }
    }
}
