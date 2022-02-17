namespace PataRoad.Core.Character.Bosses
{
    class DodongaEnemy : EnemyBossBehaviour
    {
        protected override void Init()
        {
            CharacterSize = 7;
        }
        protected override (string action, float distance) GetNextBehaviour()
        {
            //Tada. depends on level, it does nothing!
            if (Common.Utils.RandomByProbability(1f / (_level + 5)))
            {
                return ("nothing", 5);
            }
            if (Common.Utils.RandomByProbability(1f / (_level + 7)))
            {
                return ("Idle", 5);
            }
            var firstPon = _pataponsManager.FirstPatapon;
            if (firstPon?.Type != Class.ClassType.Toripon &&
                firstPon?.transform.position.x < _pataponsManager.transform.position.x) return ("fire", 20);
            if ((firstPon?.IsMeleeUnit == true || firstPon?.Type == Class.ClassType.Toripon)
                && Common.Utils.RandomByProbability((float)_pataponsManager.PataponCount / 20))
            {
                if (_level >= 3 && Common.Utils.RandomByProbability((float)_pataponsManager.PataponCount / 18))
                {
                    return ("eat", 0);
                }
                else
                {
                    return ("headbutt", 0);
                }
            }
            else
            {
                if (_level >= 10 && Common.Utils.RandomByProbability(0.5f))
                {
                    return ("growl", 5);
                }
                else
                {
                    return ("fire", 3);
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
