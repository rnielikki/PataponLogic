using PataRoad.Core.Character.Equipments.Weapons;
using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Character.Animal
{
    [DisallowMultipleComponent]
    class TotemBehaviour : MonoBehaviour, ICharacter
    {
        public CharacterAnimator CharAnimator { get; private set; }

        public DistanceCalculator DistanceCalculator { get; private set; }

        public AttackType AttackType => AttackType.Neutral;

        public ElementalAttackType ElementalAttackType => ElementalAttackType.Neutral;

        [SerializeReference]
        private Stat _stat = Stat.GetAnyDefaultStatForCharacter();
        public Stat Stat => _stat;

        [SerializeReference]
        private AttackTypeResistance _attackTypeResistance = new AttackTypeResistance();
        public AttackTypeResistance AttackTypeResistance => _attackTypeResistance;

        [SerializeField]
        float _moveDistance;

        [SerializeField]
        ParticleSystem _groundParticle;

        public int CurrentHitPoint { get; private set; }

        public StatusEffectManager StatusEffectManager { get; private set; }

        public bool IsDead { get; private set; }

        public float DefaultWorldPosition => _pataponsManagerTransform.position.x + _moveDistance / 2;
        public float MinimumWorldPosition => _pataponsManagerTransform.position.x + CharacterSize / 2;
        public float MaximumWorldPosition => _pataponsManagerTransform.position.x + _moveDistance / 2;

        public Vector2 MovingDirection => Vector2.left;
        private bool _movingLeft;

        public float AttackDistance => 0;

        public float Sight => _moveDistance;

        public float CharacterSize => 4;

        public bool UseCenterAsAttackTarget => false;

        public UnityEvent<float> OnDamageTaken => null;

        private Transform _pataponsManagerTransform;

        private bool _moving;

        private void Start()
        {
            CurrentHitPoint = Stat.HitPoint;

            DistanceCalculator = DistanceCalculator.GetNonPataHazoDistanceCalculator(this);
            _pataponsManagerTransform = FindObjectOfType<Patapons.PataponsManager>().transform;

            CharAnimator = new CharacterAnimator(GetComponent<Animator>(), this);

            StatusEffectManager = gameObject.AddComponent<CharacterStatusEffectManager>();

            StatusEffectManager.OnStatusEffect.AddListener((effect) =>
            {
                if (effect == StatusEffectType.Stagger)
                {
                    CharAnimator.Animator.Play("Idle");
                    StopWatching();
                    _moving = true;
                }
                else if (effect == StatusEffectType.Tumble)
                {
                    CharAnimator.Animate("flip");
                    StopWatching();
                    _moving = false;
                    StatusEffectManager.IgnoreStatusEffect = true;
                    _groundParticle.Stop();
                }
            });
            _moving = true;
        }
        public void PerformNormal()
        {
            if (!IsDead)
            {
                CharAnimator.Animate("Idle");
                _groundParticle.Play();
            }
        }
        public void EndStatusEffect()
        {
            StatusEffectManager.IgnoreStatusEffect = false;
            _moving = true;
        }

        public void Die()
        {
            _moving = false;
            IsDead = true;
            CharAnimator.Animator.SetBool("watching", false);
            _groundParticle.Stop();
            StatusEffectManager.RecoverAndIgnoreEffect();
            CharAnimator.PlayDyingAnimation();
            foreach (var sprite in GetComponentsInChildren<SpriteRenderer>(true))
            {
                sprite.sortingLayerName = "background";
                sprite.sortingOrder = 50;
            }

            Map.Weather.WeatherInfo.Current.ChangeWeather(Map.Weather.WeatherType.Rain);
        }

        public float GetAttackValueOffset() => 1;

        public float GetDefenceValueOffset() => 1;
        private void StartWatching()
        {
            CharAnimator.Animator.SetBool("watching", true);
            _moving = false;
            _groundParticle.Stop();
        }
        public void StopWatching()
        {
            CharAnimator.Animator.SetBool("watching", false);
            _moving = true;
            _groundParticle.Play();
        }

        public void OnAttackHit(Vector2 point, int damage)
        {
            //nothing
        }

        public void OnAttackMiss(Vector2 point)
        {
            //nothing
        }

        public void StopAttacking(bool pause)
        {
            //do you even attk lol.
        }

        public void TakeDamage(int damage)
        {
            CurrentHitPoint -= damage;
        }
        private void Update()
        {
            if (_moving)
            {
                var pos = transform.position;
                var min = MinimumWorldPosition;
                var max = MaximumWorldPosition;
                pos.x = Mathf.Clamp(transform.position.x + _stat.MovementSpeed * Time.deltaTime * (_movingLeft ? -1 : 1),
                    min, max);
                transform.position = pos;
                if (pos.x <= min)
                {
                    _movingLeft = false;
                }
                else if (pos.x >= max)
                {
                    _movingLeft = true;
                    StartWatching();
                }
            }
        }
    }
}
