namespace PataRoad.Core.Character.Bosses
{
    public class DokaknelEnemy : EnemyBossBehaviour
    {
        protected override void Init()
        {
            _minCombo = 2;
        }
        protected override BossAttackMoveSegment GetNextBehaviour()
        {
            if (_pataponsManager.ContainsClassOnly(Class.ClassType.Toripon))
            {
                if (Common.Utils.RandomByProbability(0.2f - (0.02f * _level)))
                {
                    return new BossAttackMoveSegment("fire", 1, 5);
                }
                else
                {
                    return new BossAttackMoveSegment("slam", 0, 2);
                }
            }
            else if (transform.position.x > _pataponsManager.transform.position.x + 20)
            {
                return new BossAttackMoveSegment("wheel", 15, 20);
            }
            else if (_pataponsManager.IsAllMelee())
            {
                if (Common.Utils.RandomByProbability(0.1f + (0.02f * _level)))
                {
                    return new BossAttackMoveSegment("wheel", 15, 15);
                }
                else if (Common.Utils.RandomByProbability(0.3f + (0.01f * _level)))
                {
                    return new BossAttackMoveSegment("earthquake", 2, 5);
                }
                else
                {
                    return new BossAttackMoveSegment("slam", 0, 0);
                }
            }
            else
            {
                int meleeCount = _pataponsManager.GetMeleeCount();
                if (Common.Utils.RandomByProbability(meleeCount > 1 ? 0.2f : 0.1f + (0.02f * _level)))
                {
                    return new BossAttackMoveSegment("wheel", 15, 15);
                }
                else if (Common.Utils.RandomByProbability(0.4f - (0.01f * _level)))
                {
                    return new BossAttackMoveSegment("fire", 2, 5);
                }
                else if (Common.Utils.RandomByProbability(0.5f - (0.01f * _level)))
                {
                    return new BossAttackMoveSegment("slam", 0, 0);
                }
                else
                {
                    return new BossAttackMoveSegment("earthquake", 0, 5);
                }
            }
        }
        protected override string GetNextBehaviourOnIce()
        {
            return "fire";
        }
    }
}