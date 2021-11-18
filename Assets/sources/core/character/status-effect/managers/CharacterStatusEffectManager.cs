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
        protected override void StartStatusEffect()
        {
            _character.StopAttacking();
            (_character as MonoBehaviour)?.StopAllCoroutines();
        }
        public override void SetIce(int time)
        {
            if (!IsValidForStatusEffect(time)) return;
            StartStatusEffect();
            LoadEffectObject(StatusEffectType.Ice);

            OnIce();

            _character.CharAnimator.Stop();
            StartCoroutine(WaitForRecovery(time));
            OnStatusEffect = true;
        }
        /// <summary>
        /// Called when ice effect starts, before setting <see cref="OnStatusEffect"/> to <c>true</c>.
        /// </summary>
        protected virtual void OnIce() { }

        public override void SetSleep(int time)
        {
            if (!IsValidForStatusEffect(time)) return;
            StartStatusEffect();
            _character.CharAnimator.Animate("Sleep");

            OnSleep();

            LoadEffectObject(StatusEffectType.Sleep);

            _character.CharAnimator.Stop();
            StartCoroutine(WaitForRecovery(time));

            OnStatusEffect = true;
        }
        /// <summary>
        /// Called when sleep effect starts, before setting <see cref="OnStatusEffect"/> to <c>true</c>.
        /// </summary>
        protected virtual void OnSleep() { }
        public override void SetStagger()
        {
            if (!IsValidForStatusEffect(1)) return;
            StartStatusEffect();
            _character.CharAnimator.Animate("Stagger");
            StartCoroutine(WaitForRecovery(1));

            OnStatusEffect = true;
        }
        public override void SetKnockback()
        {
            if (IgnoreStatusEffect || OnStatusEffect) return;
            _character.StopAttacking();
            (_character as MonoBehaviour)?.StopAllCoroutines();
            OnKnockback();

            OnStatusEffect = true;
        }
        /// <summary>
        /// Called when knockback effect starts, before setting <see cref="OnStatusEffect"/> to <c>true</c>.
        /// </summary>
        protected virtual void OnKnockback() { }
        protected override void OnRecover()
        {
            _character.CharAnimator.Resume();
            _character.CharAnimator.Animate("Idle");
        }
        public override void TumbleAttack()
        {
            foreach (var target in _character.DistanceCalculator.GetAllGroundedTargets())
            {
                target.StatusEffectManager.Tumble();
            }
        }
        private bool IsValidForStatusEffect(int time) => !IgnoreStatusEffect && !OnStatusEffect && time > 0;
    }
}
