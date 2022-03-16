using PataRoad.Core.Character;
using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class StructurePart : Structure
    {
        [SerializeField]
        private Structure _parent;
        [SerializeField]
        [Header("Note: This won't affect hit point")]
        private bool _useParentStat;
        protected override void Start()
        {
            base.Start();
            _animator = null;
            if (_useParentStat)
            {
                int health = Stat.HitPoint;
                Stat = _parent.Stat.Copy();
                Stat.HitPoint = health;
                _attackTypeResistance = _parent.AttackTypeResistance;
            }
            _parent.OnDestroy.AddListener(Die);
        }
        public override void Die()
        {
            base.Die();
            if (!_parent.IsDead)
            {
                _parent.RemoveSprites(transform);
            }
            Destroy(gameObject);
        }
        public override bool TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            _parent.TakeDamage(damage);
            return true;
        }
    }
}
