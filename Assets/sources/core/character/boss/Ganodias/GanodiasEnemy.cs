using static PataRoad.Common.Utils;

namespace PataRoad.Core.Character.Bosses
{
    class GanodiasEnemy : EnemyBossBehaviour
    {
        private bool _firstAttackDone;
        protected override void Init()
        {
            Boss.UseWalkingBackAnimation();
        }
        protected override BossAttackMoveSegment GetNextBehaviour()
        {
            if (!_firstAttackDone)
            {
                _firstAttackDone = true;
                if (_level == 1)
                {
                    return new BossAttackMoveSegment("cannon", 3);
                }
            }
            if (!_pataponsManager.ContainsClassOnly(Class.ClassType.Toripon)
                && RandomByProbability(0.1f + (_pataponsManager.ContainsClass(Class.ClassType.Toripon) ? 0.1f : -0.02f)))
            {
                if (RandomByProbability(1 - UnityEngine.Mathf.Sqrt(_level) * 0.15f))
                {
                    return new BossAttackMoveSegment("cannon", 3);
                }
                else
                {
                    return new BossAttackMoveSegment("bomb", 1);
                }
            }
            return MaceOrBullet();
        }
        protected override BossAttackMoveSegment GetNextBehaviourForCombo(int index, int comboCount)
        {
            //Don't use inside combo if it's not last
            // * bomb
            // * cannon
            if (index == comboCount - 1)
            {
                return GetNextBehaviour();
            }
            else return MaceOrBullet();
        }
        private BossAttackMoveSegment MaceOrBullet()
        {
            if (RandomByProbability((float)_pataponsManager.PataponCount / 28))
            {
                return new BossAttackMoveSegment("mace", 2.5f);
            }
            else
            {
                return new BossAttackMoveSegment("bullet", 0);
            }
        }
        protected override string GetNextBehaviourOnIce()
        {
            return "bullet";
        }
    }
}
