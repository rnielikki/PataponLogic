using UnityEngine;

namespace PataRoad.Core.Map.Weather
{
    class RainWeatherData : MonoBehaviour, IWeatherData
    {
        public WeatherType Type => WeatherType.Rain;
        [SerializeField]
        public AudioClip _sound;
        [SerializeField]
        private GameObject _darkBg;

        public void OnWeatherStarted(bool firstInit)
        {
            if (gameObject.activeSelf) return;
            gameObject.SetActive(true);
            WeatherInfo.Current.SetWeatherSound(_sound);
            WeatherInfo.Current.FireRateMultiplier = 0.5f;

            Character.CharacterEnvironment.AnimalSightMultiplier = 0.6f;
            if (_darkBg != null) _darkBg.SetActive(true);

            WeatherInfo.Current.Clouds.Show();
        }

        public void OnWeatherStopped(WeatherType newType)
        {
            WeatherInfo.Current.FireRateMultiplier = 1;
            WeatherInfo.Current.StopWeatherSound();
            Character.CharacterEnvironment.AnimalSightMultiplier = 1;

            gameObject.SetActive(false);
            if (_darkBg != null) _darkBg.SetActive(false);

            WeatherInfo.Current.Clouds.StartHiding();
        }
        public void StartListen(LayerMask layerMask)
        {
            var particleSystem = GetComponent<ParticleSystem>();
            var collision = particleSystem.collision;
            collision.enabled = true;
            collision.type = ParticleSystemCollisionType.World;
            collision.mode = ParticleSystemCollisionMode.Collision2D;
            collision.bounce = 0;
            collision.lifetimeLoss = 1;
            collision.enableDynamicColliders = true;
            collision.collidesWith = layerMask;
            collision.sendCollisionMessages = true;
        }

        private void OnParticleCollision(GameObject other)
        {
            other.GetComponent<IWeatherReceiver>()?.ReceiveWeather(WeatherType.Rain);
        }
    }
}
