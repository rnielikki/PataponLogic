namespace PataRoad.Core.Character.Bosses
{
    public class ShooshookleSummon : SummonedBoss
    {
        protected override void Chakachaka()
        {
            CharAnimator.Animate("slam");
        }
        protected override void ChargedChakachaka()
        {
            CharAnimator.Animate("spore");
        }
        protected override void Ponpon()
        {
            CharAnimator.Animate("slam");
        }
        protected override void ChargedPonpon()
        {
            CharAnimator.Animate("sprout");
        }
        protected override void OnStarted()
        {
            Map.Weather.WeatherInfo.Current.ChangeWeather(Map.Weather.WeatherType.Rain);
        }
        protected override void OnDead()
        {
            if (Map.Weather.WeatherInfo.Current != null)
            {
                Map.Weather.WeatherInfo.Current.EndChangingWeather();
            }
        }
    }
}