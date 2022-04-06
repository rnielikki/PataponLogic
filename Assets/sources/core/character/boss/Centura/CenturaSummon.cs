namespace PataRoad.Core.Character.Bosses
{
    public class CenturaSummon : SummonedBoss
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