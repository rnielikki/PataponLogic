using UnityEngine;

namespace PataRoad.Core.Character
{
    public class CharacterStatusEffectManager : StatusEffectManager
    {
        protected ICharacter _character;
        public bool WasAttacking { get; private set; }

        private void Awake()
        {
            Init();
        }
        protected override void Init()
        {
            base.Init();
            _character = _target as ICharacter;
        }
        protected override void StopEverythingBeforeStatusEffect(StatusEffectType type)
        {
            WasAttacking = _character.IsAttacking;
            _character.StopAttacking(false);
            if (_character is MonoBehaviour mono)
            {
                mono.StopAllCoroutines();
            }
        }
        public override void SetIce(float time)
        {
            if (!IsValidForStatusEffect(time) || time < 1) return;
            StopEverythingBeforeStatusEffect(StatusEffectType.Ice);
            LoadEffectObject(StatusEffectType.Ice);
            base.SetIce(time);

            OnIce();

            if (!IsBigTarget) _character.CharAnimator.Stop();
            StartCoroutine(WaitForRecovery(time));
            CurrentStatusEffect = StatusEffectType.Ice;
        }
        /// <summary>
        /// Called when ice effect starts, before setting <see cref="OnStatusEffect"/> to <c>true</c>.
        /// </summary>
        protected virtual void OnIce() { }

        public override void SetSleep(float time)
        {
            if (!IsValidForStatusEffect(time)) return;
            StopEverythingBeforeStatusEffect(StatusEffectType.Sleep);
            base.SetSleep(time);
            _character.CharAnimator.Animate("Sleep");

            OnSleep();

            LoadEffectObject(StatusEffectType.Sleep);
            StartCoroutine(WaitForRecovery(time));

            CurrentStatusEffect = StatusEffectType.Sleep;
        }
        /// <summary>
        /// Called when sleep effect starts, before setting <see cref="OnStatusEffect"/> to <c>true</c>.
        /// </summary>
        protected virtual void OnSleep() { }
        public override void SetStagger()
        {
            if (!IsValidForStatusEffect(1)) return;
            StopEverythingBeforeStatusEffect(StatusEffectType.Stagger);
            base.SetStagger();
            _character.CharAnimator.Animate("Stagger");
            StartCoroutine(WaitForRecovery(1));

            CurrentStatusEffect = StatusEffectType.Stagger;
        }
        public override void SetKnockback()
        {
            if (IgnoreStatusEffect || IsOnStatusEffect || _character.IsDead) return;
            _character.StopAttacking(false);
            if (_character is MonoBehaviour mono) mono.StopAllCoroutines();
            base.SetKnockback();
            OnKnockback();

            CurrentStatusEffect = StatusEffectType.Knockback;
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
        public override void TumbleAttack(bool hasDamage = false)
        {
            foreach (var target in _character.DistanceCalculator.GetAllGroundedTargets())
            {
                target.StatusEffectManager.Tumble();
                if (target is MonoBehaviour targetBehaviour && hasDamage)
                {
                    Equipments.Logic.DamageCalculator.DealDamage(
                        _character, _target.Stat, targetBehaviour.gameObject, targetBehaviour.transform.position, true);
                }
            }
        }
        private bool IsValidForStatusEffect(float time) => !_character.IsDead && !IgnoreStatusEffect && !IsOnStatusEffect && time > 0;
    }
}
