using UnityEngine;
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
        private bool _sleepUsed;
        protected override void Init()
        {
            _minCombo = 2;
            Boss.SetWalkingBackAnimationName("jump");

            FindObjectOfType<Rhythm.Command.RhythmCommand>().ComboManager.FeverManager
                .OnFeverCanceled.AddListener(() =>
                {
                    if (Boss.BossTurnManager.LeftActionCount < 2)
                    {
                        Boss.BossTurnManager.DefineNextAction("death-bubble");
                    }
                });
        }
        protected override BossAttackMoveSegment GetNextBehaviour()
        {
            return GetNextAttackExceptSleep();
        }

        protected override string GetNextBehaviourOnIce()
        {
            if (RandomByProbability(0.5f))
            {
                return "pick";
            }
            else return "slash";
        }
        protected override BossAttackMoveSegment GetNextBehaviourForCombo(int index, int comboCount)
        {
            if (index == 0 && comboCount > 0)
            {
                _sleepUsed = true;
                return new BossAttackMoveSegment("sleep-bubble", 1);
            }
            else if (_sleepUsed || index == comboCount - 1)
            {
                _sleepUsed = false;
                return GetNextAttackExceptSleep();
            }
            return base.GetNextBehaviourForCombo(index, comboCount);
        }
        private BossAttackMoveSegment GetNextAttackExceptSleep()
        {
            _sleepUsed = false;
            if (_pataponsManager.ContainsClassOnly(Class.ClassType.Toripon))
            {
                return new BossAttackMoveSegment("slash", 0);
            }
            //Without special case like combo, it's actually uncommon
            if (RandomByProbability(
                Mathf.Sqrt(_level) * 0.01f
                + (_pataponsManager.ContainsClass(Class.ClassType.Tatepon) ? 0.1f : 0.025f)
                ))
            {
                return new BossAttackMoveSegment("death-bubble", 1);
            }
            if (RandomByProbability(_pataponsManager.PataponCount * 0.02f))
            {
                return new BossAttackMoveSegment("pick", 2);
            }
            else return new BossAttackMoveSegment("slash", 0);
        }
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