using UnityEngine;

namespace PataRoad.Core.Character
{
    internal class SmallCharacterStatusEffectManager : CharacterStatusEffectManager
    {
        private CharacterSoundsCollection _soundCollection;

        private Vector2 _movingDirectionOnFire;
        private float _defaultYPosition; //for rigidbody position
        private bool _isPatapon;
        protected SmallCharacter _smallCharacter;
        private Rigidbody2D _rigidbody;
        private bool _onKnockback;
        private float _xDirection;
        private bool _isRigidbodyActive;
        private Vector2 _force = new Vector2(2400, 2200);
        private void Awake()
        {
            Init();
            _defaultYPosition = transform.position.y;
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
            _movingDirectionOnFire = _smallCharacter.MovingDirection;
        }
        protected override bool OnFireInterval()
        {
            GameSound.SpeakManager.Current.Play(_soundCollection.OnFire);
            return base.OnFireInterval();
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
            if (IsOnStatusEffect || IgnoreStatusEffect || _smallCharacter.IgnoreTumble)
            {
                return;
            }
            StopEverythingBeforeStatusEffect(StatusEffectType.Tumble);
            _smallCharacter.CharAnimator.Animate("Sleep");
            base.Tumble();
            ActivateRigidbody();
            transform.position = new Vector3(transform.position.x, transform.position.y + (Time.deltaTime * 1.2f), transform.position.z);
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
            AddForce(_force);
        }
        /// <summary>
        /// Throw with power. Warning: You will want to manually manage this as status effect or... for example, it'll attack on sky.
        /// </summary>
        /// <param name="force">Force to push.</param>
        public void AddForce(Vector2 force)
        {
            if (_smallCharacter.IsFixedPosition || _defaultYPosition > 0 || _onKnockback) return;
            _smallCharacter.CharAnimator.Animate("walk");
            ActivateRigidbody();
            transform.position = new Vector3(transform.position.x, Time.deltaTime * 2, transform.position.z);
            _rigidbody.AddForce(new Vector2(-_xDirection * force.x, force.y));
            _onKnockback = true;
        }
        private void ActivateRigidbody()
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rigidbody.gravityScale = 1;
            _isRigidbodyActive = true;
        }
        private void Update()
        {
            if (IsOnStatusEffect && !_smallCharacter.IsFixedPosition && _isOnFire && !_character.IsDead)
            {
                var offset = _smallCharacter.Stat.MovementSpeed * 1.5f * Time.deltaTime * _movingDirectionOnFire;
                Vector3 pos = transform.position + (Vector3)offset;
                float xBeforeClamp = pos.x;
                if (_smallCharacter.MovingDirection.x == _movingDirectionOnFire.x)
                {
                    pos.x = _smallCharacter.DistanceCalculator.GetSafeForwardPosition(pos.x);
                }
                if ((_xDirection * _character.DefaultWorldPosition) - CharacterEnvironment.DodgeDistance > _xDirection * pos.x
                    || _xDirection * _character.DefaultWorldPosition < _xDirection * pos.x
                    || pos.x != xBeforeClamp)
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
            if (_isRigidbodyActive && !_character.IsDead)
            {
                var pos = transform.position;
                pos.x = Mathf.Clamp(pos.x,
                    _character.DefaultWorldPosition - CharacterEnvironment.MaxAttackDistance,
                    _character.DefaultWorldPosition + CharacterEnvironment.MaxAttackDistance);

                if (transform.position.y <= _defaultYPosition)
                {
                    pos.y = _defaultYPosition;
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
