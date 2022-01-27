using UnityEngine;

namespace PataRoad.Core.Character
{
    public class CharacterStatusEffectManager : StatusEffectManager
    {
        protected ICharacter _character;

        private void Awake()
        {
            Init();
        }
        protected override void Init()
        {
            base.Init();
            _character = _target as ICharacter;
        }
        protected override void StopEverythingBeforeStatusEffect()
        {
            _character.StopAttacking(false);
            (_character as MonoBehaviour)?.StopAllCoroutines();
        }
        public override void SetIce(float time)
        {
            if (!IsValidForStatusEffect(time) || time < 1) return;
            StopEverythingBeforeStatusEffect();
            LoadEffectObject(StatusEffectType.Ice);
            base.SetIce(time);

            OnIce();

            _character.CharAnimator.Stop();
            StartCoroutine(WaitForRecovery(time));
            IsOnStatusEffect = true;
        }
        /// <summary>
        /// Called when ice effect starts, before setting <see cref="OnStatusEffect"/> to <c>true</c>.
        /// </summary>
        protected virtual void OnIce() { }

        public override void SetSleep(float time)
        {
            if (!IsValidForStatusEffect(time)) return;
            StopEverythingBeforeStatusEffect();
            base.SetSleep(time);
            _character.CharAnimator.Animate("Sleep");

            OnSleep();

            LoadEffectObject(StatusEffectType.Sleep);
            StartCoroutine(WaitForRecovery(time));

            IsOnStatusEffect = true;
        }
        /// <summary>
        /// Called when sleep effect starts, before setting <see cref="OnStatusEffect"/> to <c>true</c>.
        /// </summary>
        protected virtual void OnSleep() { }
        public override void SetStagger()
        {
            if (!IsValidForStatusEffect(1)) return;
            StopEverythingBeforeStatusEffect();
            base.SetStagger();
            _character.CharAnimator.Animate("Stagger");
            StartCoroutine(WaitForRecovery(1));

            IsOnStatusEffect = true;
        }
        public override void SetKnockback()
        {
            if (IgnoreStatusEffect || IsOnStatusEffect || _character.IsDead) return;
            _character.StopAttacking(false);
            (_character as MonoBehaviour)?.StopAllCoroutines();
            base.SetKnockback();
            OnKnockback();

            IsOnStatusEffect = true;
        }
        /// <summary>
        /// Called when knockback effect starts, before setting <see cref="OnStatusEffect"/> to <c>true</c>.
        /// </summary>
        protected virtual void OnKnockback() { }
        protected override void OnRecover()
        {
            _character.CharAnimator.Resume();
            if (!_character.IsDead) _character.CharAnimator.Animate("Idle");
        }
        public override void TumbleAttack()
        {
            foreach (var target in _character.DistanceCalculator.GetAllGroundedTargets())
            {
                target.StatusEffectManager.Tumble();
            }
        }
        private bool IsValidForStatusEffect(float time) => !_character.IsDead && !IgnoreStatusEffect && !IsOnStatusEffect && time > 0;
    }
}
