namespace PataRoad.Core.Map.Weather
{
    interface IWeatherReceiver
    {
        public void ReceiveWeather(WeatherType weatherType);
    }
}
