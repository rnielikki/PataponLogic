using PataRoad.Core.Character.Equipments.Weapons;
using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Character.Animal
{
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

        public int CurrentHitPoint { get; private set; }

        public UnityEvent<float> OnDamageTaken { get; } = new UnityEvent<float>();

        public StatusEffectManager StatusEffectManager { get; private set; }

        public bool IsDead { get; private set; }

        public float DefaultWorldPosition => transform.position.x;

        public Vector2 MovingDirection => Vector2.left;

        public float AttackDistance => 0;

        public float Sight => _animalData.Sight * CharacterEnvironment.AnimalSightMultiplier;

        private void Start()
        {
            _animalData = GetComponent<IAnimalData>();
            CurrentHitPoint = Stat.HitPoint;

            DistanceCalculator = DistanceCalculator.GetAnimalDistanceCalculator(this);
            CharAnimator = new CharacterAnimator(GetComponent<Animator>(), this);
            StatusEffectManager = gameObject.AddComponent<CharacterStatusEffectManager>();
            _animalData.InitFromParent(this);
        }

        public void Die()
        {
            IsDead = true;
            foreach (var component in GetComponentsInChildren<Collider2D>())
            {
                component.enabled = false;
            }
            StatusEffectManager.RecoverAndIgnoreEffect();
            CharAnimator.PlayDyingAnimation();
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
            CharAnimator.Animate("Idle");
        }

        public virtual void TakeDamage(int damage)
        {
            CurrentHitPoint -= damage;
            _animalData.OnDamaged();
        }
        private void Update()
        {
            if (!_animalData.PerformingAction && !IsDead && !StatusEffectManager.OnStatusEffect && DistanceCalculator.HasAttackTarget())
            {
                _animalData.OnTarget();
            }
        }
    }
}
