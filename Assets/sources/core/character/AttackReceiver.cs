using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Character
{
    public class AttackReceiver : MonoBehaviour, IAttackable
    {
        private Stat _stat = Stat.GetAnyDefaultStatForCharacter();
        [SerializeField]
        private int _hitPoint;
        public Stat Stat => _stat;

        public AttackTypeResistance AttackTypeResistance { get; } = new AttackTypeResistance();

        public int CurrentHitPoint { get; private set; }

        public UnityEvent<float> OnDamageTaken => null;

        public StatusEffectManager StatusEffectManager { get; private set; }

        public bool IsDead { get; private set; }
        [SerializeField]
        private UnityEvent _onAfterDeath;

        void Start()
        {
            _stat.HitPoint = _hitPoint;
            CurrentHitPoint = _hitPoint;

            Stat.BoostResistance(Mathf.Infinity);
            StatusEffectManager = gameObject.AddComponent<StatusEffectManager>();
        }
        public void Die()
        {
            IsDead = true;
            _onAfterDeath.Invoke();
        }

        public float GetDefenceValueOffset() => 1;
        public bool TakeDamage(int damage)
        {
            CurrentHitPoint -= damage;
            return true;
        }
    }
}