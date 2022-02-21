namespace PataRoad.Core.Character.Bosses
{
    class ZaknelSummon : SummonedBoss
    {
        protected override void Chakachaka()
        {
            CharAnimator.Animate("fire");
        }

        protected override void ChargedChakachaka()
        {
            CharAnimator.Animate("earthquake");
        }
        protected override void Ponpon()
        {
            CharAnimator.Animate("slam");
        }
        protected override void ChargedPonpon()
        {
            CharAnimator.Animate("slam");
        }

        protected override void OnDead()
        {
            Map.Weather.WeatherInfo.Current.EndChangingWeather();
        }

        protected override void OnStarted()
        {
            Map.Weather.WeatherInfo.Current.ChangeWeather(Map.Weather.WeatherType.Clear);
        }

    }
}
