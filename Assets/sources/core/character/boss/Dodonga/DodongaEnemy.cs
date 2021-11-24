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
        private (UnityEngine.Events.UnityAction action, int distance) GetNextBehaviour()
        {
            var firstPon = _pataponsManager.FirstPatapon;
            if (firstPon.transform.position.x < _pataponsManager.transform.position.x) return (_attack.AnimateFire, 20);
            if (firstPon?.IsMeleeUnit == true || firstPon?.Class == Patapons.ClassType.Toripon)
            {
                if (Common.Utils.RandomByProbability((float)_pataponsManager.PataponCount / 20))
                {
                    if (Common.Utils.RandomByProbability((float)_pataponsManager.PataponCount / 18))
                    {
                        return (_attack.AnimateEat, 10);
                    }
                    else
                    {
                        return (_attack.AnimateHeadbutt, 10);
                    }
                }
                else
                {
                    return (_attack.AnimateFire, 10);
                }
            }
            else return (_attack.AnimateFire, 15);
        }
    }
}
