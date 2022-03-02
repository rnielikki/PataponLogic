namespace PataRoad.Core.Character.Bosses
{
    class ManborothEnemy : EnemyBossBehaviour
    {
        protected override string[][] _predefinedCombos { get; set; } = new string[][]
        {
            new string[]{ "blow", "stomp" },
            new string[]{ "blow", "tackle" },
            new string[]{ "tackle", "stomp" },
            new string[]{ "blow", "tackle", "stomp" }
        };
        bool _blowAsLastMove;

        protected override void Init()
        {
            _minCombo = 2;
        }
        protected override BossAttackMoveSegment GetNextBehaviour()
        {
            bool onlyToripon = _pataponsManager.ContainsClassOnly(Class.ClassType.Toripon);
            var meleeCount = _pataponsManager.GetMeleeCount();

            if (!_blowAsLastMove && !onlyToripon && Common.Utils.RandomByProbability(0.6f - meleeCount * 0.1f - _level * 0.02f))
            {
                _blowAsLastMove = true;
                return new BossAttackMoveSegment("blow", 2, 4);
            }
            else
            {
                if (Common.Utils.RandomByProbability(0.5f + _level * 0.01f))
                {
                    return new BossAttackMoveSegment("tackle", 0, 5);
                }
                else
                {
                    return new BossAttackMoveSegment("stomp", 0, 5);
                }
            }
        }
        protected override string GetNextBehaviourOnIce()
        {
            return GetNextBehaviour().Action;
        }
    }
}
