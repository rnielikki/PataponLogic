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
        protected override void OnStarted()
        {
            Map.Weather.WeatherInfo.Current.ChangeWeather(Map.Weather.WeatherType.Fog);
        }
        protected override void OnDead()
        {
            Map.Weather.WeatherInfo.Current?.EndChangingWeather();
        }
    }
}
