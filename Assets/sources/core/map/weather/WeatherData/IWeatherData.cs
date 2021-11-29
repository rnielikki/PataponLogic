namespace PataRoad.Core.Map.Weather
{
    public interface IWeatherData
    {
        public WeatherType Type { get; }
        public void OnWeatherStarted();
        public void OnWeatherStopped();
    }
}
