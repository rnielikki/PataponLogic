namespace PataRoad.Core.Character.Bosses
{
    class DarantulaEnemy : EnemyBossBehaviour, IAbsorbableBossBehaviour
    {
        protected override string[][] _predefinedCombos { get; set; } = new string[][]
        {
            new string[]{ "poison", "absorb" },
            new string[]{ "poison", "tailwhip" },
            new string[]{ "poison", "tailslide" }
        };
        protected override void Init()
        {
            Boss.UseWalkingBackAnimation();
        }
        public void SetAbsorbHit()
        {
            Boss.CharAnimator.Animate("absorbing");
        }

        protected override BossAttackMoveSegment GetNextBehaviour()
        {
            return new BossAttackMoveSegment("poison", 0);
            if (_pataponsManager.ContainsClassOnly(Class.ClassType.Toripon))
            {
                return new BossAttackMoveSegment("tailwhip", 0);
            }
            if (_pataponsManager.IsAllMelee())
            {
                return new BossAttackMoveSegment(WhipOrSlide(), 0);
            }
            if (Common.Utils.RandomByProbability(0.2f + (_level * 0.05f)))
            {
                return new BossAttackMoveSegment("absorb", 0, 10);
            }
            if (Common.Utils.RandomByProbability(0.2f - (_level * 0.04f)))
            {
                return new BossAttackMoveSegment("poison", 0);
            }
            else
            {
                return new BossAttackMoveSegment(WhipOrSlide(), 0);
            }
        }
        private string WhipOrSlide()
        {
            if (_level < 10) return "tailwhip";
            else
            {
                if (Common.Utils.RandomByProbability(_level * 0.035f))
                {
                    return "tailslide";
                }
                else
                {
                    return "tailwhip";
                }
            }
        }

        protected override string GetNextBehaviourOnIce()
        {
            //It doesn't attk with feet
            return GetNextBehaviour().Action;
        }
        public void StopAbsorbing()
        {
            //sorry for boilerplate, but it's warm
        }
        public void Heal(int amount) => Boss.Heal(amount);
    }
}
