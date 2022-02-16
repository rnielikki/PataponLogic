using PataRoad.Core.Map.Weather;

namespace PataRoad.Core.Character.Bosses
{
    /// <summary>
    /// Reacts to specific weather. Without the weather, it won't get damage.
    /// </summary>
    class DarantulaEnemyBoss : EnemyBoss
    {
        bool _isVisible = true; //it's "visible" by default - like having normal health and opacity
        UnityEngine.SpriteRenderer[] _renderers;
        private int _savedHitPoint;
        private int _savedMaxHitPoint;
        private void ShowIfRain(WeatherType type)
        {
            if (type == WeatherType.Fog && _isVisible)
            {
                _savedHitPoint = CurrentHitPoint;
                SetMaximumHitPoint(0);

                _isVisible = false;
                SetRendererOpacity(0.3f);
                StatusEffectManager.IgnoreStatusEffect = true;
            }
            else if (!_isVisible)
            {
                Stat.HitPoint = _savedMaxHitPoint;
                CurrentHitPoint = _savedHitPoint;

                _isVisible = true;
                SetRendererOpacity(1);
                StatusEffectManager.IgnoreStatusEffect = false;
            }
        }
        public override void SetLevel(int level, int absoluteMaxLevel)
        {
            base.SetLevel(level, absoluteMaxLevel);
            _savedMaxHitPoint = Stat.HitPoint;
            WeatherInfo.Current.OnWeatherChanged.AddListener(ShowIfRain);
            _renderers = GetComponentsInChildren<UnityEngine.SpriteRenderer>();
            ShowIfRain(WeatherInfo.Current.CurrentWeather);
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
