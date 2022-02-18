
using PataRoad.Core.Character.Equipments.Weapons;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Character.Bosses
{
    public abstract class Boss : MonoBehaviour, ICharacter
    {
        public abstract Vector2 MovingDirection { get; }

        public DistanceCalculator DistanceCalculator { get; protected set; }
        public virtual float DefaultWorldPosition { get; protected set; }

        public float AttackDistance { get; protected set; }

        public Stat Stat => BossAttackData.Stat;

        public int CurrentHitPoint { get; protected set; }

        public StatusEffectManager StatusEffectManager { get; private set; }
        public bool IsDead { get; private set; }

        public CharacterAnimator CharAnimator { get; private set; }
        public BossAttackData BossAttackData { get; private set; }

        private readonly Dictionary<GameObject, BreakablePart> _breakableParts = new Dictionary<GameObject, BreakablePart>();

        public UnityEvent<float> OnDamageTaken { get; private set; }

        public AttackType AttackType { get; internal set; }

        public AttackTypeResistance AttackTypeResistance => BossAttackData.AttackTypeResistance;

        public ElementalAttackType ElementalAttackType { get; internal set; }

        public float Sight => CharacterEnvironment.Sight;

        public float CharacterSize { get; protected set; }

        public bool UseCenterAsAttackTarget => true;

        [SerializeField]
        protected UnityEvent _onAfterDeath = new UnityEvent();
        public UnityEvent OnAfterDeath => _onAfterDeath;
        public virtual void Init()
        {
            BossAttackData = GetComponent<BossAttackData>();
            foreach (var part in GetComponentsInChildren<BreakablePart>())
            {
                _breakableParts.Add(part.gameObject, part);
            }

            CharAnimator = new CharacterAnimator(GetComponent<Animator>(), this);

            BossAttackData.CharAnimator = CharAnimator;
            CurrentHitPoint = Stat.HitPoint;
            StatusEffectManager = gameObject.AddComponent<BossStatusEffectManager>();

            var health = GetComponentInChildren<HealthDisplay>();
            if (health != null)
            {
                OnDamageTaken = new UnityEvent<float>();
                OnDamageTaken.AddListener(health.UpdateBar);
            }

            CharAnimator.Animate("Idle");
        }
        public virtual void Die()
        {
            IsDead = true;
            BossAttackData.StopAllAttacking();
            foreach (var component in GetComponentsInChildren<Collider2D>(true))
            {
                component.enabled = false;
            }
            foreach (var component in GetComponentsInChildren<BossAttackComponent>(true))
            {
                component.SetDisable();
            }
            StatusEffectManager.RecoverAndIgnoreEffect();
            CharAnimator.PlayDyingAnimation();
            _onAfterDeath?.Invoke();
        }
        /// <summary>
        /// Sets max health, also fills health to full.
        /// </summary>
        internal void SetMaximumHitPoint(int point)
        {
            Stat.HitPoint = CurrentHitPoint = point;
        }
        public void Heal(int amount)
        {
            CurrentHitPoint = Mathf.Min(CurrentHitPoint + amount, Stat.HitPoint);
            OnDamageTaken.Invoke(CurrentHitPoint);
        }
        public virtual float GetAttackValueOffset() => Random.Range(BossAttackData.MinLastDamageOffset, BossAttackData.MinLastDamageOffset);
        public virtual float GetDefenceValueOffset() => Random.Range(0, 1);
        public virtual void OnAttackHit(Vector2 point, int damage)
        {
        }

        public virtual void OnAttackMiss(Vector2 point)
        {
        }

        public virtual void StopAttacking(bool pause)
        {
            BossAttackData.StopAllAttacking();
            CharAnimator.Animate("Idle");
        }

        public virtual void TakeDamage(int damage)
        {
            CurrentHitPoint -= damage;
        }
        public float GetBrokenPartMultiplier(GameObject part, int damage)
        {
            if (_breakableParts.TryGetValue(part, out BreakablePart breakable))
            {
                return breakable.TakeDamage(damage);
            }
            else return 1;
        }
    }
}
