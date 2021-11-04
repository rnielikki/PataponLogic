using UnityEngine;

namespace Core.Character
{
    public class Structure : MonoBehaviour, IAttackable
    {
        public Stat Stat { get; private set; }

        public int CurrentHitPoint { get; set; }

        [SerializeField]
        private int _hitPoint;
        [SerializeField]
        private float _defence;

        private void Awake()
        {
            Stat = new Stat
            {
                HitPoint = _hitPoint,
                Defence = _defence,
                CriticalResistance = Mathf.Infinity,
                StaggerResistance = Mathf.Infinity,
                KnockbackResistance = Mathf.Infinity,
                FireResistance = 0.25f,
                IceResistance = Mathf.Infinity,
                SleepResistance = Mathf.Infinity
            };
            CurrentHitPoint = _hitPoint;
        }
        public void Die()
        {
            Destroy(gameObject);
        }
        public void TakeDamage(int damage)
        {
            //Can attach health status or change sprite etc.
            CurrentHitPoint -= damage;
        }
    }
}
