﻿using UnityEngine;

namespace PataRoad.Core.Map.Weather
{
    class FogWeatherData : MonoBehaviour, IWeatherData
    {
        public WeatherType Type => WeatherType.Fog;

        public void OnWeatherStarted()
        {
            gameObject.SetActive(true);
            WeatherInfo.Current.StopWeatherSound();
            Character.CharacterEnvironment.Sight = Character.CharacterEnvironment.OriginalSight * 0.8f;
        }

        public void OnWeatherStopped(WeatherType newType)
        {
            Character.CharacterEnvironment.Sight = Character.CharacterEnvironment.OriginalSight;
            gameObject.SetActive(false);
        }
    }
}
