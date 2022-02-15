namespace PataRoad.Core.Character
{
    public class BossStatusEffectManager : CharacterStatusEffectManager
    {
        public override bool CanContinue => base.CanContinue || _isOnFire || CurrentStatusEffect == StatusEffectType.Ice;
        void Awake()
        {
            Init();
            IsBigTarget = true;
        }
        protected override void StopEverythingBeforeStatusEffect()
        {
            if (!_isOnFire)
            {
                _character.StopAttacking(false);
            }
        }
        //allows staggering while knockback
        public override void SetStagger()
        {
            if (CurrentStatusEffect == StatusEffectType.Knockback)
            {
                if (_character.IsDead || IgnoreStatusEffect) return;
                Recover(false);
            }
            base.SetStagger();
        }

        protected override void OnKnockback()
        {
            StopEverythingBeforeStatusEffect();
            _character.CharAnimator.Animate("Knockback");
            CurrentStatusEffect = StatusEffectType.Knockback;
            StartCoroutine(WaitForRecovery(8));
        }

        protected override void OnRecover()
        {
            _character.CharAnimator.Resume();
            if (!_character.IsDead) _character.CharAnimator.AnimateFrom("Idle");
        }
    }
}
