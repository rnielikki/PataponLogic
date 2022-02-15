namespace PataRoad.Core.Character.Bosses
{
    class DarantulaSummon : SummonedBoss
    {
        protected override void Chakachaka()
        {
            CharAnimator.Animate("poison");
        }

        protected override void ChargedChakachaka()
        {
            CharAnimator.Animate("poison");
        }

        protected override void ChargedPonpon()
        {
            CharAnimator.Animate("tailslide");
        }

        protected override void Ponpon()
        {
            CharAnimator.Animate("tailwhip");
        }
        private void Awake()
        {
            Init();
        }
        private void Start()
        {
            Map.Weather.WeatherInfo.Current.ChangeWeather(Map.Weather.WeatherType.Fog);
        }
        private void OnDestroy()
        {
            Map.Weather.WeatherInfo.Current?.EndChangingWeather();
        }
    }
}
