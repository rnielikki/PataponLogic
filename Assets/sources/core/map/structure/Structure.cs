using UnityEngine;

namespace PataRoad.Core.Character
{
    public class Structure : MonoBehaviour, IAttackable
    {
        public Stat Stat { get; private set; }

        public int CurrentHitPoint { get; set; }
        public StatusEffectManager StatusEffectManager { get; private set; }

        public bool IsDead { get; private set; }

        [SerializeField]
        private UnityEngine.Events.UnityEvent _onDestroy;

        [SerializeField]
        private int _hitPoint;
        [SerializeField]
        private float _defence;

        private void Awake()
        {
            Stat = new Stat
            {
                HitPoint = _hitPoint,
                DefenceMin = _defence,
                DefenceMax = _defence,
                CriticalResistance = Mathf.Infinity,
                StaggerResistance = Mathf.Infinity,
                KnockbackResistance = Mathf.Infinity,
                FireResistance = 0.25f,
                IceResistance = Mathf.Infinity,
                SleepResistance = Mathf.Infinity
            };
            CurrentHitPoint = _hitPoint;
            StatusEffectManager = gameObject.AddComponent<StatusEffectManager>();
        }
        public void Die()
        {
            IsDead = true;
            _onDestroy.Invoke();
            Destroy(gameObject);
        }
        public void TakeDamage(int damage)
        {
            //Can attach health status or change sprite etc.
            CurrentHitPoint -= damage;
        }

        public float GetDefenceValueOffset() => 1;
    }
}
