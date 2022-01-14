using UnityEngine;

namespace PataRoad.Core.Character
{
    /// <summary>
    /// Readjust and defines appearance randomly. Weather can affect.
    /// </summary>
    class RandomSpawn : MonoBehaviour
    {
        [SerializeField]
        float _appearanceChanceOnClear;
        [SerializeField]
        float _appearanceChanceOnRainy;
        [SerializeField]
        float _appearanceChanceOnFoggy;
        [SerializeField]
        float _appearanceChanceOnSnowy;

        private void Start()
        {
            if (!Common.Utils.RandomByProbability(GetChanceToAppear()))
            {
                gameObject.SetActive(false);
            }
        }
        private float GetChanceToAppear() =>
            Map.Weather.WeatherInfo.Current.CurrentWeather switch
            {
                Map.Weather.WeatherType.Clear => _appearanceChanceOnClear,
                Map.Weather.WeatherType.Rain => _appearanceChanceOnRainy,
                Map.Weather.WeatherType.Storm => _appearanceChanceOnRainy,
                Map.Weather.WeatherType.Snow => _appearanceChanceOnSnowy,
                Map.Weather.WeatherType.Fog => _appearanceChanceOnFoggy,
                _ => throw new System.NotImplementedException()
            };
    }
}
