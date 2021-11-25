namespace PataRoad.Core.Character
{
    public class BossStatusEffectManager : CharacterStatusEffectManager
    {
        public override bool CanContinue => base.CanContinue || _onFire;
        void Awake()
        {
            Init();
            _isBigTarget = true;
        }
        protected override void StartStatusEffect()
        {
            if (!_onFire)
            {
                _character.StopAttacking();
            }
        }

        protected override void OnKnockback()
        {
            StartStatusEffect();
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
