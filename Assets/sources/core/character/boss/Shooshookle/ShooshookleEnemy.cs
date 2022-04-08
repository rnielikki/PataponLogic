using static PataRoad.Common.Utils;

namespace PataRoad.Core.Character.Bosses
{
    public class ShooshookleEnemy : EnemyBossBehaviour, IAbsorbableBossBehaviour
    {
        private bool _absorbHit;
        private bool _usedSpore;

        protected override void Init()
        {
            Boss.StatusEffectManager.OnStatusEffect.AddListener((effect) =>
            {
                if (effect == StatusEffectType.Stagger) _usedSpore = true;
            });
        }
        protected override BossAttackMoveSegment GetNextBehaviour()
        {
            if (RandomByProbability(1 - (float)_pataponsManager.PataponCount / 25))
            {
                _usedSpore = false;
                return new BossAttackMoveSegment("eat", 1);
            }
            else return GetAnythingButEating();
        }

        protected override string GetNextBehaviourOnIce()
        {
            return "eat";
        }
        protected override BossAttackMoveSegment GetNextBehaviourForCombo(int index, int comboCount)
        {
            if (index == comboCount - 1)
            {
                return GetNextBehaviour();
            }
            else return GetAnythingButEating();
        }
        private BossAttackMoveSegment GetAnythingButEating()
        {
            int meleeCount = _pataponsManager.GetMeleeCount();
            if (RandomByProbability(((float)meleeCount / 15)
                + (_usedSpore ? 0.15f : 0)))
            {
                _usedSpore = false;
                return new BossAttackMoveSegment("slam", 0);
            }
            else if (meleeCount == 0 || _usedSpore || RandomByProbability(UnityEngine.Mathf.Sqrt(_level) / 10))
            {
                _usedSpore = false;
                return new BossAttackMoveSegment("sprout", 2);
            }
            else
            {
                _usedSpore = true;
                return new BossAttackMoveSegment("spore", 1);
            }
        }
        public void Heal(int amount)
        {
            Boss.Heal(amount);
        }
        public void AfterEating() => _absorbHit = false;
        public void AnimateAbsorbing()
        {
            if (_absorbHit) Boss.CharAnimator.Animate("ate");
        }
        public void SetAbsorbHit()
        {
            UseCustomDataPosition = true;
            _absorbHit = true;
        }
        public void StopAbsorbing()
        {
            UseCustomDataPosition = false;
            _absorbHit = false;
        }
    }
}