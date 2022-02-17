﻿using UnityEngine;

namespace PataRoad.Core.Character
{
    public class Structure : MonoBehaviour, IAttackable, Map.IHavingLevel
    {
        public Stat Stat { get; private set; }

        public int CurrentHitPoint { get; set; }
        public StatusEffectManager StatusEffectManager { get; private set; }

        public bool IsDead { get; private set; }

        [SerializeField]
        private UnityEngine.Events.UnityEvent _onDestroy;
        public UnityEngine.Events.UnityEvent OnDestroy => _onDestroy;

        [SerializeField]
        private int _hitPoint;
        [SerializeField]
        private UnityEngine.Events.UnityEvent<float> _onDamageTaken;
        public UnityEngine.Events.UnityEvent<float> OnDamageTaken => _onDamageTaken;

        [SerializeField]
        float _fireResistance;
        [SerializeReference]
        private AttackTypeResistance _attackTypeResistance = new AttackTypeResistance();
        public AttackTypeResistance AttackTypeResistance => _attackTypeResistance;
        protected Animator _animator;

        [SerializeField]
        bool _noLevelUp;

        [SerializeField]
        private Gradient _colorOverHealth;

        System.Collections.Generic.List<SpriteRenderer> _sprites = new System.Collections.Generic.List<SpriteRenderer>();

        private void Awake()
        {
            Stat = new Stat
            {
                HitPoint = _hitPoint,
                DefenceMin = 1,
                DefenceMax = 1,
                CriticalResistance = Mathf.Infinity,
                StaggerResistance = Mathf.Infinity,
                KnockbackResistance = Mathf.Infinity,
                FireResistance = _fireResistance,
                IceResistance = Mathf.Infinity,
                SleepResistance = Mathf.Infinity
            };
            CurrentHitPoint = _hitPoint;
            StatusEffectManager = gameObject.AddComponent<StatusEffectManager>();
            StatusEffectManager.IsBigTarget = true;
            _animator = GetComponent<Animator>();
        }
        private void Start()
        {
            SetSprites(transform);
        }
        public virtual void Die()
        {
            IsDead = true;
            _onDestroy.Invoke();
            _animator.Play("die");
        }
        public void TakeDamage(int damage)
        {
            //Can attach health status or change sprite etc.
            CurrentHitPoint -= damage;
            var percent = Mathf.Clamp01((float)CurrentHitPoint / Stat.HitPoint);
            foreach (var sprite in _sprites)
            {
                sprite.color = _colorOverHealth.Evaluate(percent);
            }
        }
        private void SetSprites(Transform tr)
        {
            foreach (Transform child in tr)
            {
                if (child.GetComponent<ICharacter>() != null) continue;
                var sprite = child.GetComponent<SpriteRenderer>();
                if (sprite != null) _sprites.Add(sprite);
                SetSprites(child);
            }
        }

        public float GetDefenceValueOffset() => 1;

        public virtual void SetLevel(int level, int absoluteMaxLevel)
        {
            if (_noLevelUp) return;
            Stat.HitPoint = CurrentHitPoint = Mathf.RoundToInt(Stat.HitPoint * (0.8f + 0.2f * level));
            Stat.FireResistance += (level - 1) * 0.02f;
        }
        public void DestroyThis() => Destroy(gameObject);
    }
}
