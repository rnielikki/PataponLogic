using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class BossAttackCollision : BossAttackComponent
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            _boss.Attack(this, collision.gameObject, collision.GetContact(0).point);
        }
    }
}
