using UnityEngine;

namespace PataRoad.Core.Character.Animal
{
    [DisallowMultipleComponent]
    class GancheekAnimalData : DefaultAnimalData
    {
        private Transform _managerTransform;
        private void Start()
        {
            gameObject.SetActive(false);
            _managerTransform = FindObjectOfType<Patapons.PataponsManager>().transform;
            Map.Weather.WeatherInfo.Current.OnWeatherChanged.AddListener(ShowIfRain);
        }
        private void ShowIfRain(Map.Weather.WeatherType type)
        {
            if (type == Map.Weather.WeatherType.Rain || type == Map.Weather.WeatherType.Storm)
            {
                if (_managerTransform.position.x + CharacterEnvironment.MaxAttackDistance < transform.position.x)
                {
                    gameObject.SetActive(true);
                }
                Map.Weather.WeatherInfo.Current.OnWeatherChanged.RemoveListener(ShowIfRain);
            }
        }
        public override void OnTarget()
        {
            if (CanMove())
            {
                base.OnTarget();
                _targetPosition.x = Map.MissionPoint.Current.MissionPointPosition.x;
                _useFixedTargetPosition = true;
            }
        }
        private void Update()
        {
            if (_behaviour.StatusEffectManager.CurrentStatusEffect != StatusEffectType.Stagger)
            {
                Move();
            }
        }
    }
}
