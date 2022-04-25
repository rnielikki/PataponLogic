using PataRoad.Core.Character;

namespace PataRoad.Core.Map.Levels.King
{
    public class KingEnemyData : Character.Bosses.EnemyBossBehaviour
    {
        int _index;
        protected override BossAttackMoveSegment GetNextBehaviour()
        {
            _index = (_index + 1) % 3;
            switch (_index)
            {
                case 0:
                    return new BossAttackMoveSegment("wall", 15, 15);
                case 1:
                    return new BossAttackMoveSegment("meteor", 0, 0);
                case 2:
                    return new BossAttackMoveSegment("rush", 15, 15);
                default:
                    throw new System.NotImplementedException();
            }
        }

        protected override string GetNextBehaviourOnIce()
        {
            return "nothing";
        }
    }
}