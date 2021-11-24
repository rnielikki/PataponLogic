﻿
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Character.Bosses
{
    public abstract class Boss : MonoBehaviour, ICharacter
    {
        public abstract Vector2 MovingDirection { get; }

        public DistanceCalculator DistanceCalculator { get; protected set; }

        public float AttackDistance { get; protected set; }

        public Stat Stat => BossAttackData.Stat;

        public int CurrentHitPoint { get; private set; }

        public StatusEffectManager StatusEffectManager { get; private set; }

        public bool IsDead { get; private set; }

        public CharacterAnimator CharAnimator { get; private set; }
        public BossAttackData BossAttackData { get; private set; }

        private readonly Dictionary<GameObject, BreakablePart> _breakableParts = new Dictionary<GameObject, BreakablePart>();

        [SerializeField]
        private UnityEvent<float> _onDamageTaken;
        public UnityEvent<float> OnDamageTaken => _onDamageTaken;

        protected virtual void Init(BossAttackData data)
        {
            foreach (var part in GetComponentsInChildren<BreakablePart>())
            {
                _breakableParts.Add(part.gameObject, part);
            }

            CharAnimator = new CharacterAnimator(GetComponent<Animator>(), this);

            BossAttackData = data;
            data.CharAnimator = CharAnimator;
            CurrentHitPoint = Stat.HitPoint;
            StatusEffectManager = gameObject.AddComponent<StatusEffectManager>();

            CharAnimator.Animate("Idle");
        }

        public virtual void Die()
        {
            IsDead = true;
            foreach (var component in GetComponentsInChildren<Collider2D>())
            {
                component.enabled = false;
            }

            CharAnimator.PlayDyingAnimation();
        }

        public float GetAttackValueOffset() => Random.Range(BossAttackData.MinLastDamageOffset, BossAttackData.MinLastDamageOffset);
        public float GetDefenceValueOffset() => Random.Range(0, 1);


        public virtual void OnAttackHit(Vector2 point, int damage)
        {
        }

        public virtual void OnAttackMiss(Vector2 point)
        {
        }

        public virtual void StopAttacking()
        {
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
