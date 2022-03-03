using PataRoad.Core.Character;
using PataRoad.Core.Map.Weather;
using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class Level29Candle : Structure, IWeatherReceiver
    {
        [SerializeField]
        int _rainDamage;
        [SerializeField]
        int _snowDamage;
        [SerializeField]
        float _offset;
        [SerializeField]
        ParticleSystem _particles;
        [SerializeField]
        private Stat _stat;
        Transform _transform;
        int _weatherLayer;

        protected override void InitStat()
        {
            Stat = _stat;
        }
        protected override void Start()
        {
            base.Start();
            _weatherLayer = LayerMask.NameToLayer("weather");
            _transform = Character.Patapons.PataponsManager.Current.transform;
            WeatherInfo.Current.GetComponentInChildren<RainWeatherData>(true)
                .StartListen(
                CharacterTypeDataCollection.GetCharacterData(CharacterType.Patapon).SelfLayerMask
                );
            _animator = null;
        }
        private void Update()
        {
            var pos = transform.position;
            pos.x = _transform.position.x - _offset;
            transform.position = pos;
        }
        public override void Die()
        {
            _particles.Stop();
            base.Die();
        }
        public override void TakeDamage(int damage)
        {
            CurrentHitPoint -= damage;
            if (CurrentHitPoint > 0)
            {
                var main = _particles.main;
                main.startSizeMultiplier = (float)CurrentHitPoint / _stat.HitPoint;
            }
        }
        public void ReceiveWeather(WeatherType weatherType)
        {
            switch (WeatherInfo.Current.CurrentWeather)
            {
                case WeatherType.Rain:
                case WeatherType.Storm:
                    TakeDamage(_rainDamage);
                    break;
                case WeatherType.Snow:
                    TakeDamage(_snowDamage);
                    break;
            }
            if (CurrentHitPoint <= 0) Die();
        }
    }
}
