namespace PataRoad.Core.Character.Bosses
{
    public class DodongaEnemy : EnemyBoss
    {
        private void Awake()
        {
            Init(GetComponent<DodongaAttack>());
            AttackDistance = 10;
        }
        protected override float CalculateAttack()
        {
            var (action, distance) = GetNextBehaviour();
            BossTurnManager
                .SetOneAction(action);

            //combo test!
            (action, _) = GetNextBehaviour();
            BossTurnManager
                .SetOneAction(action);
            //combo test!
            (action, distance) = GetNextBehaviour();
            BossTurnManager
                .SetOneAction(action);
            return distance;
        }
        //Example
        private (string action, int distance) GetNextBehaviour()
        {
            //Tada. depends on level, it does nothing!
            if (Common.Utils.RandomByProbability(1f / (_level + 2)))
            {
                return ("nothing", 10);
            }
            if (Common.Utils.RandomByProbability(1f / (_level + 5)))
            {
                return ("Idle", 10);
            }
            var firstPon = _pataponsManager.FirstPatapon;
            if (firstPon?.Class != Class.ClassType.Toripon &&
                firstPon?.transform.position.x < _pataponsManager.transform.position.x) return ("fire", 20);
            if (firstPon?.IsMeleeUnit == true || firstPon?.Class == Class.ClassType.Toripon)
            {
                if (Common.Utils.RandomByProbability((float)_pataponsManager.PataponCount / 20))
                {
                    if (Common.Utils.RandomByProbability((float)_pataponsManager.PataponCount / 18))
                    {
                        return ("eat", 10);
                    }
                    else
                    {
                        return ("headbutt", 10);
                    }
                }
                else
                {
                    return ("fire", 10);
                }
            }
            else return ("fire", 15);
        }
    }
}
