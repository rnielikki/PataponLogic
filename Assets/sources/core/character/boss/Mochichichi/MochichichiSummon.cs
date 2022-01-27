namespace PataRoad.Core.Character.Bosses
{
    class MochichichiSummon : SummonedBoss
    {
        private void Start()
        {
            Map.Weather.WeatherInfo.Current.Wind.StartWind(Map.Weather.WindType.TailWind);
        }
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
        private void Awake()
        {
            Init();
        }
        private void OnDestroy()
        {
            Map.Weather.WeatherInfo.Current?.Wind?.StopWind(Map.Weather.WindType.TailWind);
        }
    }
}
