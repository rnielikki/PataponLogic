using System;
using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class BossAttackCollision : BossAttackComponent
    {
        private bool _enabled;
        [SerializeField]
        bool _enabledFromFirst;
        private void Awake()
        {
            Init();
            _enabled = _enabledFromFirst;
        }
        internal void Attack()
        {
            _enabled = true;
        }
        public override void StopAttacking()
        {
            _enabled = false;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_enabled) _boss.Attack(this, collision.gameObject, collision.GetContact(0).point);
        }
    }
}
