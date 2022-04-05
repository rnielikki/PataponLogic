namespace PataRoad.Core.Character.Bosses
{
    class DodongaEnemy : EnemyBossBehaviour
    {
        protected override BossAttackMoveSegment GetNextBehaviour()
        {
            //Tada. depends on level, it does nothing!
            if (Common.Utils.RandomByProbability(1f / (_level + 5)))
            {
                return new BossAttackMoveSegment("nothing", 5);
            }
            if (Common.Utils.RandomByProbability(1f / (_level + 7)))
            {
                return new BossAttackMoveSegment("Idle", 5);
            }
            var firstPon = _pataponsManager.FirstPatapon;
            if (firstPon == null || (firstPon.Type != Class.ClassType.Toripon &&
                firstPon.transform.position.x < _pataponsManager.transform.position.x))
            {
                return new BossAttackMoveSegment("fire", 5);
            }
            if ((firstPon.IsMeleeUnit || firstPon.Type == Class.ClassType.Toripon)
                && Common.Utils.RandomByProbability((float)_pataponsManager.PataponCount / 20))
            {
                if (_level >= 3 && Common.Utils.RandomByProbability((float)_pataponsManager.PataponCount / 18))
                {
                    return new BossAttackMoveSegment("eat", 0);
                }
                else
                {
                    return new BossAttackMoveSegment("headbutt", 0);
                }
            }
            else
            {
                if (_level >= 10 && Common.Utils.RandomByProbability(0.5f))
                {
                    return new BossAttackMoveSegment("growl", 5);
                }
                else
                {
                    return new BossAttackMoveSegment("fire", 3);
                }
            }
        }

        protected override string GetNextBehaviourOnIce()
        {
            if (_level < 10) return "nothing";
            else return "growl";
        }
    }
}
