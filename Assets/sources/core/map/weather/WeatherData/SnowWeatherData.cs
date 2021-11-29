namespace PataRoad.Core.Map.Weather
{
    class SnowWeatherData : UnityEngine.MonoBehaviour, IWeatherData
    {
        public WeatherType Type => WeatherType.Snow;

        public void OnWeatherStarted()
        {
            gameObject.SetActive(true);
            WeatherInfo.Current.FireRateMultiplier = 0.25f;
            WeatherInfo.Current.IceRateMultiplier = 1.5f;
        }

        public void OnWeatherStopped()
        {
            WeatherInfo.Current.FireRateMultiplier = 1;
            WeatherInfo.Current.IceRateMultiplier = 1;
            gameObject.SetActive(false);
        }
        private void OnParticleCollision(UnityEngine.GameObject other)
        {
            if (other.tag != "SmallCharacter") return;
            var character = other.gameObject.GetComponentInParent<Character.ICharacter>();
            if (character != null
                &&
                !character.StatusEffectManager.OnStatusEffect
                &&
                Common.Utils.RandomByProbability((1 - character.Stat.IceResistance) * 0.02f))
            {
                character.StatusEffectManager.SetIce(4);
            }
        }
    }
}
