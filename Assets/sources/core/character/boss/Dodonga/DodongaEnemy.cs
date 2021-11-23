using System.Linq;

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
        protected override void CalculateAttack()
        {
            BossTurnManager
                .SetOneAction(GetNextBehaviour())
                ?.StartAttack();

        }
        //Example
        private UnityEngine.Events.UnityAction GetNextBehaviour()
        {
            var firstPon = _pataponsManager.FirstPatapon;
            if (firstPon?.IsMeleeUnit == true || firstPon?.Class == Patapons.ClassType.Toripon)
            {
                if (Common.Utils.RandomByProbability((float)_pataponsManager.PataponCount / 20))
                {
                    if (Common.Utils.RandomByProbability((float)_pataponsManager.PataponCount / 18))
                    {
                        return _attack.AnimateEat;
                    }
                    else
                    {
                        return _attack.AnimateHeadbutt;
                    }
                }
                else
                {
                    return _attack.AnimateFire;
                }
            }
            else return _attack.AnimateFire;
        }
    }
}
