namespace PataRoad.Core.Character.Bosses
{
    public class KacchindongaSummon : SummonedBoss
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
            CharAnimator.Animate("growl");
        }
        protected override void OnStarted()
        {
            Map.Weather.WeatherInfo.Current.ChangeWeather(Map.Weather.WeatherType.Snow);
            Map.Weather.WeatherInfo.Current.Wind.StartWind(Map.Weather.WindType.TailWind);
        }
        protected override void OnDead()
        {
            if (Map.Weather.WeatherInfo.Current != null)
            {
                Map.Weather.WeatherInfo.Current.EndChangingWeather();
                if (Map.Weather.WeatherInfo.Current.Wind != null)
                {
                    Map.Weather.WeatherInfo.Current.Wind.StopWind(Map.Weather.WindType.TailWind);
                }
            }
        }
    }
}