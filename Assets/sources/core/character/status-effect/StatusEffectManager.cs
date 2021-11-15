using PataRoad.Core.Character.Equipments.Logic;
using System.Collections;
using UnityEngine;

namespace PataRoad.Core.Character
{
    public class StatusEffectManager : MonoBehaviour
    {
        private ICharacter _character;
        public bool OnStatusEffect { get; private set; }
        public bool IgnoreStatusEffect { get; set; }
        private bool _onFire;

        private bool _isCharacter;
        private bool _isSmallCharacter;
        private Transform _transform;

        private CharacterSoundsCollection _soundCollection;

        private GameObject _effectObject;
        private StatusEffectData _effectInstantiator;

        private Vector2 _positionOnStatusEffect;

        private Vector2 _movingDirectionOnFire;
        private Transform _bodyTransform;

        private void Awake()
        {
            _character = GetComponent<ICharacter>();
            _isCharacter = _character != null;
            if (_character is SmallCharacter small)
            {
                _isSmallCharacter = true;
                _soundCollection = small.Sounds;
                if (small.IsFlyingUnit)
                {
                    _transform = transform.Find(small.RootName);
                }
            }
            if (_transform == null) _transform = transform;

            _effectInstantiator = FindObjectOfType<StatusEffectData>();
        }
        public void SetFire(int time)
        {
            if (OnStatusEffect || time < 1 || IgnoreStatusEffect) return;
            StartStatusEffect();
            _onFire = true;

            StartCoroutine(FireDamage());
            if (_isSmallCharacter)
            {
                _character.CharAnimator.Animate("Fire");
                _movingDirectionOnFire = -_character.MovingDirection;
            }
            LoadEffectObject(StatusEffectType.Fire);
            OnStatusEffect = true;

            IEnumerator FireDamage()
            {
                int timeSum = 0;
                while (timeSum < time)
                {
                    DamageCalculator.DealDamageFromFireEffect(_character, gameObject, _transform);
                    GameSound.SpeakManager.Current.Play(_soundCollection?.OnFire);
                    yield return new WaitForSeconds(1);
                    timeSum++;
                }
                Recover();
            }
        }
        public void SetIce(int time)
        {
            if (!IsValidForStatusEffect(time)) return;
            StartStatusEffect();
            LoadEffectObject(StatusEffectType.Ice);

            if (_character.IsFlyingUnit)
            {
                _character.CharAnimator.Animate("tori-fly-stop");
            }

            _character.CharAnimator.Stop();
            StartCoroutine(WaitForRecovery(time));
            OnStatusEffect = true;
        }
        public void SetSleep(int time)
        {
            if (!IsValidForStatusEffect(time)) return;
            StartStatusEffect();
            _character.CharAnimator.Animate("Sleep");
            GameSound.SpeakManager.Current.Play(_soundCollection?.OnSleep);
            LoadEffectObject(StatusEffectType.Sleep);

            if (_character.IsFlyingUnit)
            {
                _character.CharAnimator.Animate("tori-fly-stop");
            }
            _character.CharAnimator.Stop();
            StartCoroutine(WaitForRecovery(time));

            OnStatusEffect = true;
        }
        public void SetStagger()
        {
            if (!IsValidForStatusEffect(1)) return;
            StartStatusEffect();
            _character.CharAnimator.Animate("Stagger");
            StartCoroutine(WaitForRecovery(1));

            OnStatusEffect = true;
        }
        public void Tumble()
        {
            //check "is grounded" but how?
            if (!_isSmallCharacter || OnStatusEffect || IgnoreStatusEffect) return;
            StartStatusEffect();
            _character.CharAnimator.Animate("Sleep");
            Rhythm.Command.TurnCounter.OnNextTurn.AddListener(Recover);

            OnStatusEffect = true;
        }
        public void Recover()
        {
            if (!OnStatusEffect || IgnoreStatusEffect) return;
            StopAllCoroutines();
            _onFire = false;
            if (_effectObject != null)
            {
                Destroy(_effectObject);
            }
            OnStatusEffect = false;
            _character?.CharAnimator?.Resume();
            _character?.CharAnimator?.Animate("Idle");

            if (!(_character is Patapons.Patapon patapon) || patapon.OnFever)
            {
                _character.CharAnimator.AnimateFrom("tori-fly-up");
            }
            else
            {
                _character.CharAnimator.AnimateFrom("tori-fly-down");
            }
        }
        private void LoadEffectObject(StatusEffectType type)
        {
            //can be resized if big, I guess
            _effectObject = _effectInstantiator.AttachEffect(type, _transform);
        }
        private void StartStatusEffect()
        {
            _character?.StopAttacking();
            (_character as MonoBehaviour)?.StopAllCoroutines();
            _positionOnStatusEffect = transform.position;
        }

        private bool IsValidForStatusEffect(int time) => _isCharacter && !IgnoreStatusEffect && !OnStatusEffect && time > 0;
        private IEnumerator WaitForRecovery(int seconds)
        {
            yield return new WaitForSeconds(seconds);
            Recover();
        }
        private void Update()
        {
            if (OnStatusEffect && _isCharacter)
            {
                if (_onFire && _isSmallCharacter)
                {
                    var dir = _character.MovingDirection.x;
                    var offset = _character.Stat.MovementSpeed * 1.5f * Time.deltaTime * _movingDirectionOnFire;
                    var pos = transform.position + (Vector3)offset;

                    if (dir * _positionOnStatusEffect.x - CharacterEnvironment.DodgeDistance > dir * pos.x
                        || dir * _positionOnStatusEffect.x < dir * pos.x)
                    {
                        _movingDirectionOnFire = -_movingDirectionOnFire;
                        transform.position -= (Vector3)offset;
                    }
                    else
                    {
                        transform.position = pos;
                    }
                }
                else
                {
                    transform.position = _positionOnStatusEffect;
                }
            }
        }
    }
}
