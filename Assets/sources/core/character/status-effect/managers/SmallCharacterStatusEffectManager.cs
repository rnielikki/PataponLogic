﻿using System.Collections;
using UnityEngine;

namespace PataRoad.Core.Character
{
    internal class SmallCharacterStatusEffectManager : CharacterStatusEffectManager
    {
        private CharacterSoundsCollection _soundCollection;

        private Vector2 _movingDirectionOnFire;
        private Vector2 _positionOnStatusEffect;
        private bool _isPatapon;
        protected SmallCharacter _smallCharacter;
        private Rigidbody2D _rigidbody;
        private bool _onKnockback;
        private float _xDirection;
        private void Awake()
        {
            Init();
        }
        protected override void Init()
        {
            base.Init();
            _smallCharacter = _target as SmallCharacter;
            _xDirection = _smallCharacter.MovingDirection.x;
            if (_smallCharacter.IsFlyingUnit) _transform = transform.Find(_smallCharacter.RootName);
            _soundCollection = _smallCharacter.Sounds;
            _isPatapon = _smallCharacter is Patapons.Patapon;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        protected override void OnFire()
        {
            _smallCharacter.CharAnimator.Animate("Fire");
            _movingDirectionOnFire = -_smallCharacter.MovingDirection;
            StartCoroutine(SpeakOnFire());
            IEnumerator SpeakOnFire()
            {
                while (OnStatusEffect)
                {
                    GameSound.SpeakManager.Current.Play(_soundCollection.OnFire);
                    yield return new WaitForSeconds(1);
                }
            }
        }
        protected override void OnIce()
        {
            if (_smallCharacter.IsFlyingUnit)
            {
                _smallCharacter.CharAnimator.Animate("tori-fly-stop");
            }
        }
        protected override void OnSleep()
        {
            GameSound.SpeakManager.Current.Play(_soundCollection.OnSleep);
            if (_smallCharacter.IsFlyingUnit)
            {
                _smallCharacter.CharAnimator.Animate("tori-fly-stop");
            }
        }
        public override void Tumble()
        {
            //check "is grounded" but how?
            if (OnStatusEffect || IgnoreStatusEffect) return;
            StartStatusEffect();
            _smallCharacter.CharAnimator.Animate("Sleep");
            Rhythm.Command.TurnCounter.OnNextTurn.AddListener(Recover);

            OnStatusEffect = true;
        }
        protected override void OnRecover()
        {
            base.OnRecover();
            if (_isPatapon && _smallCharacter.IsFlyingUnit)
            {
                if (((Patapons.Patapon)_smallCharacter).OnFever)
                {
                    _smallCharacter.CharAnimator.AnimateFrom("tori-fly-up");
                }
                else
                {
                    _smallCharacter.CharAnimator.AnimateFrom("tori-fly-down");
                }
            }
        }
        protected override void OnKnockback()
        {
            _smallCharacter.CharAnimator.Animate("walk");
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rigidbody.gravityScale = 1;
            transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * 1.2f, transform.position.z);
            _rigidbody.AddForce(1000 * new Vector2(-_xDirection, 2));
            _onKnockback = true;
        }
        protected override void StartStatusEffect()
        {
            base.StartStatusEffect();
            _positionOnStatusEffect = transform.position;
        }
        private void Update()
        {
            if (OnStatusEffect)
            {
                if (_onFire)
                {
                    var offset = _smallCharacter.Stat.MovementSpeed * 1.5f * Time.deltaTime * _movingDirectionOnFire;
                    var pos = transform.position + (Vector3)offset;

                    if (_xDirection * _positionOnStatusEffect.x - CharacterEnvironment.DodgeDistance > _xDirection * pos.x
                        || _xDirection * _positionOnStatusEffect.x < _xDirection * pos.x)
                    {
                        _movingDirectionOnFire = -_movingDirectionOnFire;
                        transform.position -= (Vector3)offset;
                    }
                    else
                    {
                        transform.position = pos;
                    }
                }
                else if (!_onKnockback)
                {
                    transform.position = _positionOnStatusEffect;
                }
            }
        }
        private void LateUpdate()
        {
            if (_onKnockback)
            {
                var defaultWorldPosition = _smallCharacter.AttackMoveData.DefaultWorldPosition;
                var pos = transform.position;
                if (_xDirection > 0)
                {
                    pos.x = Mathf.Clamp(transform.position.x, defaultWorldPosition - CharacterEnvironment.DodgeDistance, defaultWorldPosition);
                }
                else if (_xDirection < 0)
                {
                    pos.x = Mathf.Clamp(transform.position.x, defaultWorldPosition, defaultWorldPosition + CharacterEnvironment.DodgeDistance);
                }
                if (transform.position.y <= 0)
                {
                    pos.y = 0;
                    _rigidbody.velocity = Vector2.zero;
                    _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                    _rigidbody.gravityScale = 0;
                    _onKnockback = false;
                    Recover();
                }
                transform.position = pos;
            }
        }
    }
}
