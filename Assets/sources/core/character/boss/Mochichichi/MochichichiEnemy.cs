namespace PataRoad.Core.Character.Bosses
{
    class MochichichiEnemy : EnemyBossBehaviour
    {
        protected override void Init()
        {
            Boss.UseWalkingBackAnimation();
        }
        //Example
        protected override BossAttackMoveSegment GetNextBehaviour()
        {
            //Tada. depends on level, it does nothing!
            if (Common.Utils.RandomByProbability(1f / (_level + 5)))
            {
                return new BossAttackMoveSegment("nothing", 1);
            }
            //Toripon. Any other attacks just don't work. Don't even troll the birb with birb.
            //Note: dondon now cannot evade peck!
            var firstPon = _pataponsManager.FirstPatapon;
            bool isMeleeUnit = false;
            if (firstPon != null)
            {
                isMeleeUnit = firstPon.IsMeleeUnit;
                if (firstPon.Type == Class.ClassType.Toripon)
                {
                    if (_level < 10 || Common.Utils.RandomByProbability(0.5f + 0.01f * _level))
                    {
                        return new BossAttackMoveSegment("peck", 1);
                    }
                    else return new BossAttackMoveSegment("tornado", 1);
                }
            }

            if (_level >= 5 &&
                _pataponsManager.transform.position.x > transform.position.x)
            {
                if (Common.Utils.RandomByProbability(isMeleeUnit ? 0.6f : 0.3f))
                {
                    return new BossAttackMoveSegment("tornado", 0);
                }
                else
                {
                    return new BossAttackMoveSegment("slam", 0);
                }
            }
            var slamProbability = _pataponsManager.FirstPatapon.IsMeleeUnit ? 0.6f : 0.4f;
            if (Common.Utils.RandomByProbability(slamProbability))
            {
                return new BossAttackMoveSegment("slam", 0);
            }
            else
            {
                return new BossAttackMoveSegment("peck", 1);
            }
        }
        protected override string GetNextBehaviourOnIce() => "peck";
        protected override void OnStatusEffect(StatusEffectType type)
        {
            if (type == StatusEffectType.Stagger)
            {
                Boss.BossTurnManager.DefineNextAction("fart");
                UseCustomDataPosition = true;
            }
            else base.OnStatusEffect(type);
        }
    }
}
