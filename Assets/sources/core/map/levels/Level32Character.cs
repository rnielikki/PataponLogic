using PataRoad.Core.Character;
using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Map.Levels
{
    public class Level32Character : MonoBehaviour, IAttackable
    {
        [SerializeField]
        private Stat _stat;
        public Stat Stat => _stat;
        public AttackTypeResistance AttackTypeResistance { get; } = new AttackTypeResistance();

        public int CurrentHitPoint { get; private set; }

        [SerializeField]
        private UnityEvent<float> _onDamageTaken;
        public UnityEvent<float> OnDamageTaken => _onDamageTaken;
        [SerializeField]
        private UnityEvent _onDead;

        public StatusEffectManager StatusEffectManager { get; private set; }
        private Animator _animator;
        [SerializeField]
        private RuntimeAnimatorController _animationToChange;

        public bool IsDead { get; private set; }
        private void Awake()
        {
            StatusEffectManager = gameObject.AddComponent<StatusEffectManager>();
            _animator = GetComponent<Animator>();
        }
        private void Start()
        {
            transform.parent = Character.Patapons.PataponsManager.Current.transform;
            transform.position = Vector2.zero;
            CurrentHitPoint = _stat.HitPoint;
        }

        public void Die()
        {
            IsDead = true;
            _onDead.Invoke();
            _animator.runtimeAnimatorController = _animationToChange;
            _animator.Play("die");
        }

        public float GetDefenceValueOffset() => 1;

        public void TakeDamage(int damage)
        {
            CurrentHitPoint -= damage;
        }
    }
}