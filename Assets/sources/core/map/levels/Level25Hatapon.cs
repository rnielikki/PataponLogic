using PataRoad.Core.Character;
using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Map.Levels
{
    class Level25Hatapon : MonoBehaviour, IAttackable
    {
        [SerializeField]
        private Stat _stat;
        public Stat Stat => _stat;

        [SerializeField]
        private AttackTypeResistance _attackTypeResistance;
        public AttackTypeResistance AttackTypeResistance => _attackTypeResistance;

        public int CurrentHitPoint { get; private set; }

        [SerializeField]
        private UnityEvent<float> _onDamageTaken;
        public UnityEvent<float> OnDamageTaken => _onDamageTaken;
        [SerializeField]
        private UnityEvent _onDie;

        public StatusEffectManager StatusEffectManager { get; private set; }

        public bool IsDead { get; private set; }
        [SerializeField]
        AudioClip _deadSound;

        private Animator _animator;
        private void Start()
        {
            _animator = GetComponent<Animator>();
            StatusEffectManager = gameObject.AddComponent<StatusEffectManager>();
            StatusEffectManager.OnStatusEffect.AddListener((effect) =>
            {
                if (effect == StatusEffectType.Fire) _animator.Play("fire");
            });
            StatusEffectManager.AddRecoverAction(() =>
            {
                if (!IsDead) _animator.Play("Idle");
            });

            CurrentHitPoint = _stat.HitPoint;
        }
        public void Die()
        {
            if (IsDead) return;
            IsDead = true;
            GameSound.SpeakManager.Current.Play(_deadSound);
            _onDie.Invoke();
            _animator.Play("die");
        }

        public float GetDefenceValueOffset()
        {
            return 1;
        }

        public void TakeDamage(int damage)
        {
            CurrentHitPoint -= damage;
        }
    }
}
