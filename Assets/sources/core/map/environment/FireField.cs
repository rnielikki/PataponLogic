using PataRoad.Core.Map.Weather;
using UnityEngine;

namespace PataRoad.Core.Map.Environment
{
    class FireField : TriggerZone
    {
        [SerializeField]
        ParticleSystem _particle;
        [SerializeField]
        SpriteRenderer _hotImage;
        private void Start()
        {
            Init();
            WeatherInfo.Current.OnWeatherChanged.AddListener(ShowIfNoRain);
            ShowIfNoRain(WeatherInfo.Current.CurrentWeather);
        }

        private void ShowIfNoRain(WeatherType type)
        {
            if ((type == WeatherType.Rain || type == WeatherType.Storm || type == WeatherType.Snow)
                && _enabled)
            {
                SetDisable();
            }
            else if (!_enabled)
            {
                SetEnable();
            }
        }
        protected override void OnFirstEnter()
        {
            _hotImage.enabled = true;
        }

        protected override void OnLastExit()
        {
            _hotImage.enabled = false;
        }
        protected override void OnStay(System.Collections.Generic.IEnumerable<Character.ICharacter> currentStayingCharacters)
        {
            foreach (var receiver in currentStayingCharacters)
            {
                if (!receiver.StatusEffectManager.IsOnStatusEffect)
                {
                    Character.Equipments.Logic.DamageCalculator.CalculateAndSetStatusEffect(receiver, Character.StatusEffectType.Fire, 0.2f, receiver.Stat.FireResistance);
                    if (receiver is MonoBehaviour mono)
                    {
                        if (Character.Equipments.Logic.DamageCalculator.DealDamageFromFireEffect(receiver, mono.gameObject, mono.transform))
                        {
                            receiver.Die();
                        }
                    }
                }
            }
        }

        protected override void OnStarted(System.Collections.Generic.IEnumerable<Character.ICharacter> currentStayingCharacters)
        {
            WeatherInfo.Current.FireRateMultiplier = 1.5f;
            WeatherInfo.Current.IceRateMultiplier = 0.1f;

            var particleColor = _particle.colorOverLifetime;
            particleColor.enabled = true;
        }

        protected override void OnEnded(System.Collections.Generic.IEnumerable<Character.ICharacter> currentStayingCharacters)
        {
            var particleColor = _particle.colorOverLifetime;
            particleColor.enabled = false;

            foreach (var character in currentStayingCharacters)
            {
                if (character.StatusEffectManager.CurrentStatusEffect == Character.StatusEffectType.Fire)
                {
                    character.StatusEffectManager.Recover();
                }
            }
        }
    }
}
