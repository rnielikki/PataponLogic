using PataRoad.Core.Map.Weather;

namespace PataRoad.Core.Character.Bosses
{
    /// <summary>
    /// Reacts to specific weather. Without the weather, it won't get damage.
    /// </summary>
    class DarantulaEnemyBoss : EnemyBoss
    {
        bool _isVisible;
        UnityEngine.SpriteRenderer[] _renderers;
        private void Start()
        {
            WeatherInfo.Current.OnWeatherChanged.AddListener(ShowIfRain);
            _renderers = GetComponentsInChildren<UnityEngine.SpriteRenderer>();
            ShowIfRain(WeatherInfo.Current.CurrentWeather);
        }
        private void ShowIfRain(WeatherType type)
        {
            if (type == WeatherType.Rain || type == WeatherType.Storm)
            {
                SetRendererOpacity(1);
                _isVisible = true;
                StatusEffectManager.IgnoreStatusEffect = false;
            }
            else
            {
                SetRendererOpacity(0.3f);
                _isVisible = false;
                StatusEffectManager.IgnoreStatusEffect = true;
            }
        }
        public override void TakeDamage(int damage)
        {
            if (_isVisible) base.TakeDamage(damage);
        }
        private void SetRendererOpacity(float opacity)
        {
            foreach (var renderer in _renderers)
            {
                renderer.color = new UnityEngine.Color(1, 1, 1, opacity);
            }
        }
    }
}
