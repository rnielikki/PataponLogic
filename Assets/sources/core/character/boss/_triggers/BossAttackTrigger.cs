using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class BossAttackTrigger : BossAttackComponent
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            _boss.Attack(this, collision.gameObject, collision.ClosestPoint(transform.position));
        }
    }
}
