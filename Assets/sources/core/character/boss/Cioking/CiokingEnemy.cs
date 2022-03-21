using static PataRoad.Common.Utils;

namespace PataRoad.Core.Character.Bosses
{
    public class CiokingEnemy : EnemyBossBehaviour, IAbsorbableBossBehaviour
    {
        protected override string[][] _predefinedCombos => new string[][]
        {
            new string[]{ "sleep-bubble", "slash" },
            new string[]{ "sleep-bubble", "death-bubble" }
        };
        protected override void Init()
        {
            //_minCombo = 2;
            Boss.SetWalkingBackAnimationName("jump");
        }
        protected override BossAttackMoveSegment GetNextBehaviour()
        {
            //return new BossAttackMoveSegment("death-bubble", 0);
            //return new BossAttackMoveSegment("sleep-bubble", 0);
            return new BossAttackMoveSegment("slash", 0);
        }

        protected override string GetNextBehaviourOnIce()
        {
            if (RandomByProbability(0.5f))
            {
                return "pick";
            }
            else return "slash";
        }
        /*
        protected override BossAttackMoveSegment GetNextBehaviourForCombo(int index, int comboCount)
        {
            if (index == 0)
            {
            }
            else if (index == comboCount - 1)
            {
                return SomethingNonBubbleAttack;
            }
            return base.GetNextBehaviourForCombo(index, comboCount);
        }
        */
        public void Heal(int amount)
        {
            //No eat, just throw throw
        }

        public void SetAbsorbHit()
        {
            Boss.CharAnimator.Animate("throw");
        }

        public void StopAbsorbing()
        {
            //sorry for boilerplate, but it's warm
        }
    }
}