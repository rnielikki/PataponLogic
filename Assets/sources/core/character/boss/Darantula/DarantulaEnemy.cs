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
                return ("tailwhip", 0);
            }
            if (_pataponsManager.IsAllMelee())
            {
                return (WhipOrSlide(), 0);
            }
            if (Common.Utils.RandomByProbability(0.2f + _level * 0.05f))
            {
                return ("absorb", 0);
            }
            if (Common.Utils.RandomByProbability(0.2f - _level * 0.04f))
            {
                return ("poison", 0);
            }
            else
            {
                return (WhipOrSlide(), 0);
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
            return GetNextBehaviour().action;
        }
        internal void Heal(int amount) => _boss.Heal(amount);

        protected override void Init()
        {
            _boss.UseWalkingBackAnimation();
            CharacterSize = 10;
        }
    }
}
