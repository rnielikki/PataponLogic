namespace PataRoad.Core.Character.Bosses
{
    class MochichichiSummon : SummonedBoss
    {
        protected override void Ponpon()
        {
            CharAnimator.Animate("peck");
        }
        protected override void ChargedPonpon()
        {
            CharAnimator.Animate("slam");
        }
        protected override void Chakachaka()
        {
            CharAnimator.Animate("fart");
        }
        protected override void ChargedChakachaka()
        {
            CharAnimator.Animate("tornado");
        }
        protected override void OnStarted()
        {
            Map.Weather.WeatherInfo.Current.Wind.StartWind(Map.Weather.WindType.TailWind);
        }
        protected override void OnDead()
        {
            Map.Weather.WeatherInfo.Current?.Wind?.StopWind(Map.Weather.WindType.TailWind);
        }
    }
}
