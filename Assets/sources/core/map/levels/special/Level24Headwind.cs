using PataRoad.Core.Character;
using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class Level24Headwind : Environment.TriggerZone
    {
        private bool _isOn;

        private void Start()
        {
            Init();
            SetEnable();
        }
        public void Stop() => SetDisable();
        protected override void OnFirstEnter()
        {
            if (!_isOn)
            {
                Weather.WeatherInfo.Current.Wind.StartWind(Weather.WindType.HeadWind);
                _isOn = true;
            }
        }
        protected override void OnLastExit()
        {
            if (_isOn)
            {
                Weather.WeatherInfo.Current.Wind.StopWind(Weather.WindType.HeadWind);
                _isOn = false;
            }
        }
        protected override void OnStarted(IEnumerable<ICharacter> currentStayingCharacters)
        {
            //nothing.
        }

        protected override void OnStay(IEnumerable<ICharacter> currentStayingCharacters)
        {
            //nothing.
        }
        protected override void OnEnded(IEnumerable<ICharacter> currentStayingCharacters)
        {
            //nothing.
        }
    }
}
