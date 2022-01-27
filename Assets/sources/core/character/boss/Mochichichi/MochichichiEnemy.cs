namespace PataRoad.Core.Character.Bosses
{
    class MochichichiEnemy : EnemyBoss
    {
        private void Awake()
        {
            Init();
            CharacterSize = 4;
        }
        protected override float CalculateAttack()
        {
            string action;
            int distance = 0;
            //from level3 it will do combo attk
            var comboCount = UnityEngine.Random.Range(1,
                UnityEngine.Mathf.RoundToInt(UnityEngine.Mathf.Sqrt(_level)));

            for (int i = 0; i < comboCount; i++)
            {
                (action, distance) = GetNextBehaviour();
                BossTurnManager
                    .SetOneAction(action);
            }
            return distance;
        }
        //Example
        private (string action, int distance) GetNextBehaviour()
        {
            //Tada. depends on level, it does nothing!
            if (Common.Utils.RandomByProbability(1f / (_level + 2)))
            {
                return ("nothing", 5);
            }
            if (Common.Utils.RandomByProbability(1f / (_level + 5)))
            {
                return ("Idle", 5);
            }
            var firstPon = _pataponsManager.FirstPatapon;
            if (firstPon?.Type != Class.ClassType.Toripon &&
                firstPon?.transform.position.x < _pataponsManager.transform.position.x) return ("fart", 20);
            if (firstPon?.IsMeleeUnit == true || firstPon?.Type == Class.ClassType.Toripon)
            {
                if (Common.Utils.RandomByProbability((float)_pataponsManager.PataponCount / 20))
                {
                    if (_level >= 3 && Common.Utils.RandomByProbability((float)_pataponsManager.PataponCount / 18))
                    {
                        return ("tornado", 0);
                    }
                    else
                    {
                        return ("tornado", 0);
                    }
                }
                else if (_level >= 10 && Common.Utils.RandomByProbability(0.5f))
                {
                    return ("slam", 5);
                }
                else
                {
                    return ("peck", 3);
                }
            }
            else return ("peck", 5);
        }
    }
}
