namespace PataRoad.Core.Character.Bosses
{
    internal class ZaknelEnemy : EnemyBossBehaviour
    {
        protected override BossAttackMoveSegment GetNextBehaviour()
        {
            if (_pataponsManager.ContainsClassOnly(Class.ClassType.Toripon))
            {
                if (Common.Utils.RandomByProbability(0.5f - (0.02f * _level)))
                {
                    return new BossAttackMoveSegment("fire", 1, 5);
                }
                else
                {
                    return new BossAttackMoveSegment("slam", 0, 2);
                }
            }
            else if (_level >= 10
                && transform.position.x > _pataponsManager.transform.position.x + 20)
            {
                return new BossAttackMoveSegment("wheel", 15, 20);
            }
            else if (_pataponsManager.IsAllMelee())
            {
                if (_level >= 10 && Common.Utils.RandomByProbability(0.1f + (0.02f * _level)))
                {
                    return new BossAttackMoveSegment("wheel", 15, 15);
                }
                else if (Common.Utils.RandomByProbability(0.5f - (0.02f * _level)))
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
                if (_level >= 10
                    && Common.Utils.RandomByProbability(meleeCount > 1 ? 0.15f : 0.05f + (0.02f * _level)))
                {
                    return new BossAttackMoveSegment("wheel", 15, 15);
                }
                else if (Common.Utils.RandomByProbability(meleeCount > 0 ? 0.3f : 0.6f - (0.01f * _level)))
                {
                    return new BossAttackMoveSegment("fire", 2, 5);
                }
                else if (Common.Utils.RandomByProbability(0.5f + (0.01f * _level)))
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
