using PataRoad.Core.Character.Bosses;

namespace PataRoad.Core.Map.Levels.King
{
    public class KingBoss : EnemyBoss
    {
        //no moving back
        public override bool TakeDamage(int damage)
        {
            CurrentHitPoint -= damage;
            return true;
        }
    }
}