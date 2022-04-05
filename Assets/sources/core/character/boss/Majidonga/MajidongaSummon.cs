namespace PataRoad.Core.Character.Bosses
{
    public class MajidongaSummon : SummonedBoss
    {
        protected override void Chakachaka()
        {
            CharAnimator.Animate("fire");
        }

        protected override void ChargedChakachaka()
        {
            CharAnimator.Animate("fire");
        }

        protected override void ChargedPonpon()
        {
            CharAnimator.Animate("growl");
        }
        protected override void Ponpon()
        {
            CharAnimator.Animate("headbutt");
        }
        protected override void OnStarted()
        {
            Map.Weather.WeatherInfo.Current.ChangeWeather(Map.Weather.WeatherType.Fog);
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