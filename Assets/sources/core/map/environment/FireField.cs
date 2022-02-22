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
        private readonly HashSet<Character.ICharacter> _enteredCharacters = new HashSet<Character.ICharacter>();
        private readonly List<Character.ICharacter> _deadCharacters = new List<Character.ICharacter>();
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
                if (_characters.Count > 0)
                {
                    _hotImage.enabled = true;
                }
                Rhythm.RhythmTimer.Current.OnTime.AddListener(UpdateStatusEffect);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
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
                    if (receiver.OnAfterDeath != null && !_enteredCharacters.Contains(receiver))
                    {
                        _enteredCharacters.Add(receiver);
                        receiver.OnAfterDeath.AddListener(() =>
                        {
                            if (_characters.Contains(receiver))
                            {
                                _characters.Remove(receiver);
                            }
                        });
                    }
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "SmallCharacter")
            {
                var receiver = collision.gameObject.GetComponentInParent<Character.ICharacter>();
                if (receiver != null && _characters.Contains(receiver))
                {
                    if (!receiver.IsDead) _characters.Remove(receiver);
                    else _deadCharacters.Add(receiver);
                    if (_characters.Count == 0)
                    {
                        _hotImage.enabled = false;
                    }
                }
            }
        }
        private void UpdateStatusEffect()
        {
            if (!_enabled) return;
            foreach (var receiver in _characters)
            {
                if (!receiver.StatusEffectManager.IsOnStatusEffect)
                {
                    Character.Equipments.Logic.DamageCalculator.CalculateAndSetStatusEffect(receiver, Character.StatusEffectType.Fire, 0.2f, receiver.Stat.FireResistance);
                    if (receiver is MonoBehaviour mono)
                    {
                        Character.Equipments.Logic.DamageCalculator.DealDamageFromFireEffect(receiver, mono.gameObject, mono.transform);
                    }
                }
            }
            //collection shouldn't be changed while iterating
            if (_deadCharacters.Count > 0)
            {
                foreach (var character in _deadCharacters)
                {
                    _characters.Remove(character);
                }
                _deadCharacters.Clear();
            }
        }
    }
}
