using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class BossAttackTrigger : BossAttackComponent
    {
        Collider2D _collider;
        bool _enabled;
        private void Awake()
        {
            GetComponent<Collider2D>();
        }
        internal void Attack()
        {
            _enabled = true;
            _collider.isTrigger = true;
        }
        public override void StopAttacking()
        {
            _enabled = false;
            _collider.isTrigger = false;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_enabled) _boss.Attack(this, collision.gameObject, collision.ClosestPoint(transform.position));
        }
    }
}
