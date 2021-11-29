namespace PataRoad.Core.Map.Weather
{
    class FogWeatherData : UnityEngine.MonoBehaviour, IWeatherData
    {
        public WeatherType Type => WeatherType.Fog;

        public void OnWeatherStarted()
        {
            gameObject.SetActive(true);
            Character.CharacterEnvironment.Sight = Character.CharacterEnvironment.OriginalSight * 0.8f;
        }

        public void OnWeatherStopped()
        {
            Character.CharacterEnvironment.Sight = Character.CharacterEnvironment.OriginalSight;
            gameObject.SetActive(false);
        }
    }
}
