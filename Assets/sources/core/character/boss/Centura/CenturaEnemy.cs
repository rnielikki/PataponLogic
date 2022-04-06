namespace PataRoad.Core.Character.Bosses
{
    class CenturaEnemy : DarantulaEnemy
    {
        protected override void Init()
        {
            _minCombo = 2;
            base.Init();
        }
        protected override BossAttackMoveSegment GetNextBehaviour()
        {
            if (_pataponsManager.ContainsClassOnly(Class.ClassType.Toripon))
            {
                return new BossAttackMoveSegment("tailwhip", 0);
            }
            if (_pataponsManager.IsAllMelee())
            {
                return new BossAttackMoveSegment(WhipOrSlide(), 0);
            }
            if (Common.Utils.RandomByProbability(0.2f + (_level * 0.01f)))
            {
                return new BossAttackMoveSegment("absorb", 0, 10);
            }
            if (Common.Utils.RandomByProbability(0.1f - (_level * 0.01f)))
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

            if (Common.Utils.RandomByProbability(_level * 0.05f))
            {
                return "tailslide";
            }
            else
            {
                return "tailwhip";
            }
        }

        protected override string GetNextBehaviourOnIce()
        {
            //It doesn't attk with feet
            return GetNextBehaviour().Action;
        }
    }
}