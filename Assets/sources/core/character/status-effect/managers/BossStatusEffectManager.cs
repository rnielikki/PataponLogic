namespace PataRoad.Core.Character
{
    public class BossStatusEffectManager : CharacterStatusEffectManager
    {
        public override bool CanContinue => base.CanContinue || _isOnFire || CurrentStatusEffect == StatusEffectType.Ice;
        void Awake()
        {
            IsBigTarget = true;
            Init();
        }
        protected override void StopEverythingBeforeStatusEffect(StatusEffectType type)
        {
            if (type != StatusEffectType.Fire)
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
            StopEverythingBeforeStatusEffect(StatusEffectType.Knockback);
            _character.CharAnimator.Animate("Knockback");
            CurrentStatusEffect = StatusEffectType.Knockback;
            StartCoroutine(WaitForRecovery(8));
        }

        protected override void OnRecover(StatusEffectType type)
        {
            _character.CharAnimator.Resume();
            if (!_character.IsDead && type != StatusEffectType.Fire && type != StatusEffectType.Ice)
            {
                _character.CharAnimator.Animate("Idle");
            }
        }
    }
}
