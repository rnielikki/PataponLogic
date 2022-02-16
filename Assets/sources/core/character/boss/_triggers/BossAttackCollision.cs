using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class BossAttackCollision : BossAttackComponent
    {
        [SerializeField]
        bool _allowZeroDamage;
        private void Awake()
        {
            Init();
        }
        public override void StopAttacking()
        {
            _enabled = false;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_enabled)
            {
                _boss.Attack(this, collision.gameObject, collision.GetContact(0).point,
                    _attackType, _elementalAttackType, _allowZeroDamage);
            }
        }
    }
}
