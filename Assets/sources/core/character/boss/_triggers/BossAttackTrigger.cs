using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class BossAttackTrigger : BossAttackComponent
    {
        Collider2D _collider;
        bool _isAlreadyTrigger;
        [SerializeField]
        bool _allowZeroDamage;
        private void Awake()
        {
            Init();
            _collider = GetComponent<Collider2D>();
            _isAlreadyTrigger = _collider.isTrigger;
        }
        internal void Attack()
        {
            _enabled = true;
            if (!_isAlreadyTrigger) _collider.isTrigger = true;
        }
        public override void StopAttacking()
        {
            _enabled = false;
            if (_collider != null && !_isAlreadyTrigger)
            {
                _collider.isTrigger = false;
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (_enabled)
            {
                _boss.Attack(this, collision.gameObject, collision.ClosestPoint(transform.position),
                    _attackType, _elementalAttackType, _allowZeroDamage);
            }
        }
    }
}
