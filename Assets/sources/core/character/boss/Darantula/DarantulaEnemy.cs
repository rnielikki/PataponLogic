namespace PataRoad.Core.Character.Bosses
{
    class DarantulaEnemy : EnemyBossBehaviour
    {
        protected override string[][] _predefinedCombos { get; set; } = new string[][]
        {
            new string[]{ "poison", "absorb" },
            new string[]{ "poison", "tailwhip" },
            new string[]{ "poison", "tailslide" }
        };
        public void SetAbsorbHit()
        {
            _boss.CharAnimator.Animate("absorbing");
        }

        protected override (string action, float distance) GetNextBehaviour()
        {
            if (_pataponsManager.ContainsClassOnly(Class.ClassType.Toripon))
            {
                return ("tailwhip", 10);
            }
            if (_level >= 10)
            {
                if (Common.Utils.RandomByProbability(_level * 0.05f))
                {
                    return ("tailslide", 0);
                }
                else if (Common.Utils.RandomByProbability(1 - _level * 0.05f))
                {
                    return ("tailwhip", 0);
                }
            }
            if (_pataponsManager.IsAllMelee() || Common.Utils.RandomByProbability(0.4f + _level * 0.05f))
            {
                return ("tailwhip", 0);
            }
            if (Common.Utils.RandomByProbability(0.2f - _level * 0.04f))
            {
                return ("poison", 0);
            }
            else
            {
                return ("absorb", 0);
            }
        }

        protected override string GetNextBehaviourOnIce()
        {
            //It doesn't attk with feet
            return GetNextBehaviour().action;
        }
        internal void Heal(int amount) => _boss.Heal(amount);

        protected override void Init()
        {
            CharacterSize = 10;
        }
    }
}
