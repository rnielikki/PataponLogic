using PataRoad.Core.Character.Class;
using static PataRoad.Common.Utils;

namespace PataRoad.Core.Character.Bosses
{
    public class GaruruEnemy : EnemyBossBehaviour, IAbsorbableBossBehaviour
    {
        private GaruruAttack _garuruAttack;
        protected override void Init()
        {
            _garuruAttack = Boss.BossAttackData as GaruruAttack;
        }
        public void Heal(int amount)
        {
            //no heal, just burn
        }

        public void SetAbsorbHit()
        {
            UseCustomDataPosition = true;
            Boss.CharAnimator.Animate("burning");
        }
        public void StopAbsorbing()
        {
            UseCustomDataPosition = false;
        }

        protected override BossAttackMoveSegment GetNextBehaviour()
        {
            return _garuruAttack.IsMonsterForm ?
                GetMonsterBehaviour() : GetDragonBehaviour();
        }
        private BossAttackMoveSegment GetDragonBehaviour()
        {
            if (_pataponsManager.ContainsClassOnly(ClassType.Toripon)
                || _pataponsManager.GetMeleeCount() == 0
                || !RandomByProbability(_pataponsManager.PataponCount * 0.05f))
            {
                return RandomByProbability(0.9f) ?
                    new BossAttackMoveSegment("ball", 0) :
                    new BossAttackMoveSegment("ice", 1);
            }
            else return new BossAttackMoveSegment("burn", 0);
        }
        private BossAttackMoveSegment GetMonsterBehaviour()
        {
            if (_pataponsManager.ContainsClassOnly(ClassType.Toripon)
                && Rhythm.Command.RhythmFever.IsFever)
            {
                return new BossAttackMoveSegment("poison", 0);
            }
            if (RandomByProbability(_pataponsManager.GetMeleeCount() * 0.02f + 0.1f))
            {
                return new BossAttackMoveSegment("-rush", 0);
            }
            if (RandomByProbability(
                _pataponsManager.GetMeleeCount() * 0.02f
                + (50 - _level) * 0.002f
                ))
            {
                return new BossAttackMoveSegment("poison", 0);
            }
            return new BossAttackMoveSegment("laser", 3);
        }

        protected override BossAttackMoveSegment GetNextBehaviourForCombo(int index, int comboCount)
        {
            var next = base.GetNextBehaviourForCombo(index, comboCount);
            if (index == comboCount - 1)
            {
                return next;
            }
            else if (next.Action == "-rush")
            {
                if (RandomByProbability(
                    _pataponsManager.GetMeleeCount() * 0.02f
                    + (50 - _level) * 0.002f
                    ))
                {
                    return new BossAttackMoveSegment("poison", 0);
                }
                return new BossAttackMoveSegment("laser", 3);
            }
            else return next;
        }
        protected override string GetNextBehaviourOnIce()
        {
            //ice attack is impossible but...
            return GetNextBehaviour().Action;
        }
    }
}