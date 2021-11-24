namespace PataRoad.Core.Character.Bosses
{
    public class DodongaEnemy : EnemyBoss
    {
        private DodongaAttack _attack;
        private void Awake()
        {
            Init(GetComponent<DodongaAttack>());
            _attack = BossAttackData as DodongaAttack;
            AttackDistance = 10;
        }
        protected override float CalculateAttack()
        {
            var (action, distance) = GetNextBehaviour();
            BossTurnManager
                .SetOneAction(action);
            return distance;
        }
        //Example
        private (string action, int distance) GetNextBehaviour()
        {
            var firstPon = _pataponsManager.FirstPatapon;
            if (firstPon?.Class != Patapons.ClassType.Toripon &&
                firstPon?.transform.position.x < _pataponsManager.transform.position.x) return ("fire", 20);
            if (firstPon?.IsMeleeUnit == true || firstPon?.Class == Patapons.ClassType.Toripon)
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
