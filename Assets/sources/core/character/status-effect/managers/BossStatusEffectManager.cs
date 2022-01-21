namespace PataRoad.Core.Character
{
    public class BossStatusEffectManager : CharacterStatusEffectManager
    {
        public override bool CanContinue => base.CanContinue || _isOnFire;
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

        protected override void OnKnockback()
        {
            StopEverythingBeforeStatusEffect();
            _character.CharAnimator.Animate("Knockback");
            StartCoroutine(WaitForRecovery(8));
        }

        protected override void OnRecover()
        {
            _character.CharAnimator.Resume();
            if (!_character.IsDead) _character.CharAnimator.AnimateFrom("Idle");
        }
    }
}
