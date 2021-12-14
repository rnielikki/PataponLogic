namespace PataRoad.Core.Character.Patapons.General
{
    class PanPakaponEffect : IGeneralEffect
    {
        public void StartSelfEffect(Patapon patapon)
        {
            //check PanPakaHorn.cs weapon to see the self effect.
        }
        public void StartGroupEffect(System.Collections.Generic.IEnumerable<Patapon> patapons)
        {
            Map.Weather.WeatherInfo.Current.Wind.StartWind(Map.Weather.WindType.TailWind);
        }
        public void EndGroupEffect(System.Collections.Generic.IEnumerable<Patapon> patapons)
        {
            Map.Weather.WeatherInfo.Current?.Wind?.StopWind(Map.Weather.WindType.TailWind);
        }
    }
}
