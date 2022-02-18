using PataRoad.Core.Character.Equipments.Weapons;
using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Character.Animal
{
    [DisallowMultipleComponent]
    class AnimalBehaviour : MonoBehaviour, ICharacter
    {
        public CharacterAnimator CharAnimator { get; private set; }

        public DistanceCalculator DistanceCalculator { get; private set; }

        protected IAnimalData _animalData;

        public AttackType AttackType => _animalData.AttackType;

        public ElementalAttackType ElementalAttackType => ElementalAttackType.Neutral;

        public Stat Stat => _animalData.Stat;

        [SerializeReference]
        private AttackTypeResistance _attackTypeResistance = new AttackTypeResistance();
        public AttackTypeResistance AttackTypeResistance => _attackTypeResistance;

        [SerializeField]
        private UnityEvent _onAfterDeath;
        public UnityEvent OnAfterDeath => _onAfterDeath;

        public int CurrentHitPoint { get; private set; }

        public UnityEvent<float> OnDamageTaken { get; } = new UnityEvent<float>();

        public StatusEffectManager StatusEffectManager { get; private set; }

        public bool IsDead { get; private set; }

        public float DefaultWorldPosition { get; private set; }

        public Vector2 MovingDirection => Vector2.left;

        public float AttackDistance => 0;

        public float Sight => _animalData.Sight * CharacterEnvironment.AnimalSightMultiplier;

        [SerializeField]
        private float _characterSize;
        public float CharacterSize => _characterSize;

        public bool UseCenterAsAttackTarget => true;

        [SerializeReference]
        AudioClip _soundOnStaggered;
        [SerializeReference]
        AudioClip _soundOnDead;

        private void Start()
        {
            _animalData = GetComponent<IAnimalData>();
            CurrentHitPoint = Stat.HitPoint;

            SetCurrentAsWorldPosition();
            DistanceCalculator = DistanceCalculator.GetAnimalDistanceCalculator(this);
            CharAnimator = new CharacterAnimator(GetComponent<Animator>(), this);

            StatusEffectManager = gameObject.AddComponent<CharacterStatusEffectManager>();
            StatusEffectManager.OnStatusEffect.AddListener((effect) =>
            {
                if (effect == StatusEffectType.Stagger)
                {
                    GameSound.SpeakManager.Current.Play(_soundOnStaggered);
                }
            });
            _animalData.InitFromParent(this);
        }
        public void SetCurrentAsWorldPosition() => DefaultWorldPosition = transform.position.x;
        public void Die()
        {
            IsDead = true;
            foreach (var component in GetComponentsInChildren<Collider2D>())
            {
                component.enabled = false;
            }
            StatusEffectManager.RecoverAndIgnoreEffect();
            CharAnimator.PlayDyingAnimation();
            GameSound.SpeakManager.Current.Play(_soundOnDead);
            Map.MissionPoint.Current.FilledMissionCondition = true;
        }
        /// <summary>
        /// This is attached as animation event.
        /// </summary>
        public void CallAfterDeath()
        {
            _onAfterDeath?.Invoke();
            Destroy(gameObject);
        }

        public float GetAttackValueOffset()
        {
            return Random.Range(0, 1);
        }

        public float GetDefenceValueOffset()
        {
            return Random.Range(0, 1);
        }

        public virtual void OnAttackHit(Vector2 point, int damage)
        {
            //hmm...
        }

        public virtual void OnAttackMiss(Vector2 point)
        {
            //I think they don't care, right?
        }

        public virtual void StopAttacking(bool pause)
        {
            _animalData.StopAttacking();
            CharAnimator.Animate("Idle");
        }

        public virtual void TakeDamage(int damage)
        {
            CurrentHitPoint -= damage;
            _animalData.OnDamaged();
        }
        private void Update()
        {
            if (_animalData.CanMove() && DistanceCalculator.HasAttackTarget())
            {
                _animalData.OnTarget();
            }
        }
    }
}
