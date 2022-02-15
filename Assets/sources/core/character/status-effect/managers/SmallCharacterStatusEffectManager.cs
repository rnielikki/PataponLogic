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
        private bool _isRigidbodyActive;
        private void Awake()
        {
            Init();
            IsBigTarget = false;
        }
        protected override void Init()
        {
            base.Init();
            _smallCharacter = _target as SmallCharacter;
            _xDirection = _smallCharacter.MovingDirection.x;
            if (!string.IsNullOrEmpty(_smallCharacter.RootName)) _transform = transform.Find(_smallCharacter.RootName);
            _soundCollection = _smallCharacter.Sounds;
            _isPatapon = _smallCharacter is Patapons.Patapon;
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        protected override void OnFire()
        {
            _smallCharacter.CharAnimator.Animate("Fire");
            _movingDirectionOnFire = -_smallCharacter.MovingDirection;
        }
        protected override void OnFireInterval()
        {
            GameSound.SpeakManager.Current.Play(_soundCollection.OnFire);
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
            if (IsOnStatusEffect || IgnoreStatusEffect) return;
            StopEverythingBeforeStatusEffect(StatusEffectType.Tumble);
            _smallCharacter.CharAnimator.Animate("Sleep");
            base.Tumble();
            ActivateRigidbody();
            transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * 1.2f, transform.position.z);
            _rigidbody.AddForce(1000 * Vector2.up);
            CurrentStatusEffect = StatusEffectType.Tumble;

            StartCoroutine(WaitForRecovery(2));
        }
        protected override void OnRecover()
        {
            base.OnRecover();
            _onKnockback = false;
            if (_isPatapon && _smallCharacter.IsFlyingUnit)
            {
                if (_smallCharacter.OnFever)
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
            if (_smallCharacter.IsFixedPosition) return;
            _smallCharacter.CharAnimator.Animate("walk");
            ActivateRigidbody();
            transform.position = new Vector3(transform.position.x, Time.deltaTime * 2, transform.position.z);
            _rigidbody.AddForce(new Vector2(-_xDirection * 2400, 2200));
            _onKnockback = true;
        }
        protected override void StopEverythingBeforeStatusEffect(StatusEffectType type)
        {
            base.StopEverythingBeforeStatusEffect(type);
            _positionOnStatusEffect = transform.position;
        }

        private void ActivateRigidbody()
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rigidbody.gravityScale = 1;
            _isRigidbodyActive = true;
        }
        private void Update()
        {
            if (IsOnStatusEffect && !_smallCharacter.IsFixedPosition && _isOnFire)
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
        }
        private void LateUpdate()
        {
            if (_isRigidbodyActive)
            {
                var pos = transform.position;
                pos.x = Mathf.Clamp(pos.x,
                    _character.DefaultWorldPosition - CharacterEnvironment.MaxAttackDistance,
                    _character.DefaultWorldPosition + CharacterEnvironment.MaxAttackDistance);

                if (transform.position.y <= 0)
                {
                    pos.y = 0;
                    _rigidbody.velocity = Vector2.zero;
                    _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                    _rigidbody.gravityScale = 0;
                    _isRigidbodyActive = false;
                    if (_onKnockback)
                    {
                        Recover();
                    }
                }
                transform.position = pos;
            }
        }
    }
}
