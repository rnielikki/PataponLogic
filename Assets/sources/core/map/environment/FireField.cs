using PataRoad.Core.Map.Weather;
using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Map.Environment
{
    class FireField : MonoBehaviour
    {
        [SerializeField]
        ParticleSystem _particle;
        [SerializeField]
        SpriteRenderer _hotImage;
        private bool _enabled;
        private readonly List<Character.ICharacter> _characters = new List<Character.ICharacter>();
        private void Start()
        {
            WeatherInfo.Current.OnWeatherChanged.AddListener(ShowIfNoRain);
            ShowIfNoRain(WeatherInfo.Current.CurrentWeather);
        }

        private void ShowIfNoRain(WeatherType type)
        {
            if ((type == WeatherType.Rain || type == WeatherType.Storm || type == WeatherType.Snow)
                && _enabled)
            {
                _enabled = false;
                var particleColor = _particle.colorOverLifetime;
                particleColor.enabled = false;
                Rhythm.RhythmTimer.Current.OnTime.RemoveListener(UpdateStatusEffect);
                foreach (var character in _characters)
                {
                    if (character.StatusEffectManager.CurrentStatusEffect == Character.StatusEffectType.Fire)
                    {
                        character.StatusEffectManager.Recover();
                    }
                }
            }
            else if (!_enabled)
            {
                _enabled = true;
                var particleColor = _particle.colorOverLifetime;
                particleColor.enabled = true;
                WeatherInfo.Current.FireRateMultiplier = 1.5f;
                WeatherInfo.Current.IceRateMultiplier = 0.1f;
                Rhythm.RhythmTimer.Current.OnTime.AddListener(UpdateStatusEffect);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_enabled) return;
            if (collision.gameObject.tag == "SmallCharacter")
            {
                var receiver = collision.gameObject.GetComponentInParent<Character.ICharacter>();
                if (receiver != null && !_characters.Contains(receiver))
                {
                    if (_characters.Count == 0)
                    {
                        _hotImage.enabled = true;
                    }
                    _characters.Add(receiver);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!_enabled) return;
            if (collision.gameObject.tag == "SmallCharacter")
            {
                var receiver = collision.gameObject.GetComponentInParent<Character.ICharacter>();
                if (receiver != null && _characters.Contains(receiver))
                {
                    _characters.Remove(receiver);
                    if (_characters.Count == 0)
                    {
                        _hotImage.enabled = false;
                    }
                }
            }
        }
        private void UpdateStatusEffect()
        {
            foreach (var receiver in _characters)
            {
                if (!receiver.StatusEffectManager.IsOnStatusEffect)
                {
                    Character.Equipments.Logic.DamageCalculator.CalculateAndSetStatusEffect(receiver, Character.StatusEffectType.Fire, 1, receiver.Stat.FireResistance);
                }
            }
        }
    }
}
