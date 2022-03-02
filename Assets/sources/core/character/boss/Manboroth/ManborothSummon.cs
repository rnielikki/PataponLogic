namespace PataRoad.Core.Character.Bosses
{
    class ManborothSummon : SummonedBoss
    {
        protected override void Chakachaka()
        {
            CharAnimator.Animate("blow");
        }

        protected override void ChargedChakachaka()
        {
            CharAnimator.Animate("blow");
        }
        protected override void Ponpon()
        {
            CharAnimator.Animate("stomp");
        }

        protected override void ChargedPonpon()
        {
            CharAnimator.Animate("tackle");
        }

        protected override void OnStarted()
        {
            Map.Weather.WeatherInfo.Current.ChangeWeather(Map.Weather.WeatherType.Snow);
        }

        protected override void OnDead()
        {
            Map.Weather.WeatherInfo.Current?.EndChangingWeather();
        }
    }
}
