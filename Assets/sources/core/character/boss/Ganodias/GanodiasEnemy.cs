namespace PataRoad.Core.Character.Bosses
{
    class GanodiasEnemy : EnemyBossBehaviour
    {
        private bool _bombUsed;
        protected override BossAttackMoveSegment GetNextBehaviour()
        {
            if (!_bombUsed)
            {
                _bombUsed = true;
                return new BossAttackMoveSegment("mace", 2);
            }
            else
            {
                _bombUsed = false;
                return new BossAttackMoveSegment("bullet", 2);
            }
        }
        protected override string GetNextBehaviourOnIce()
        {
            return "bullet";
        }
    }
}
