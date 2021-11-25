using PataRoad.Core.Character.Equipments.Logic;
using System.Collections;
using UnityEngine;

namespace PataRoad.Core.Character
{
    public class StatusEffectManager : MonoBehaviour
    {
        protected IAttackable _target;
        public bool OnStatusEffect { get; protected set; }
        public bool IgnoreStatusEffect { get; set; }
        protected bool _onFire;

        protected Transform _transform;

        private GameObject _effectObject;
        private StatusEffectData _effectInstantiator;
        public virtual bool CanContinue => !OnStatusEffect && !_target.IsDead;

        private UnityEngine.Events.UnityAction _onRecover;

        private void Awake()
        {
            Init();
        }
        protected virtual void Init()
        {
            _target = GetComponent<IAttackable>();
            _effectInstantiator = FindObjectOfType<StatusEffectData>();
            _transform = transform;
        }
        public void SetRecoverAction(UnityEngine.Events.UnityAction action) => _onRecover = action;
        public virtual void SetFire(int time)
        {
            if (OnStatusEffect || time < 1 || IgnoreStatusEffect) return;
            _onFire = true;
            StartStatusEffect();

            StartCoroutine(FireDamage());
            OnFire();

            LoadEffectObject(StatusEffectType.Fire);
            OnStatusEffect = true;

            IEnumerator FireDamage()
            {
                while (time > 0)
                {
                    OnFireInterval();
                    yield return new WaitForSeconds(1);
                    time--;
                }
                Recover();
            }
        }
        /// <summary>
        /// Called when fire effect starts, before setting <see cref="OnStatusEffect"/> to <c>true</c>.
        /// </summary>
        protected virtual void OnFire() { }
        /// <summary>
        /// Every time on fire interval. Usually it's "when damage taken from fire" (which is impelemented in base)
        /// </summary>
        protected virtual void OnFireInterval()
        {
            DamageCalculator.DealDamageFromFireEffect(_target, gameObject, _transform);
        }
        public virtual void SetIce(int time) { }
        public virtual void SetSleep(int time)
        {
        }
        public virtual void SetStagger() { }
        public virtual void SetKnockback() { }
        public virtual void Tumble() { }
        public virtual void TumbleAttack() { }

        public void Recover()
        {
            if (!OnStatusEffect || IgnoreStatusEffect) return;
            StopAllCoroutines();
            _onFire = false;
            if (_effectObject != null)
            {
                Destroy(_effectObject);
            }
            OnStatusEffect = false;
            OnRecover();

            if (_onRecover != null) _onRecover();
        }

        /// <summary>
        /// Called when being recovered, before setting <see cref="OnStatusEffect"/> to <c>false</c>.
        /// </summary>
        protected virtual void OnRecover() { }
        protected void LoadEffectObject(StatusEffectType type)
        {
            //can be resized if big, I guess
            _effectObject = _effectInstantiator.AttachEffect(type, _transform);
        }
        protected virtual void StartStatusEffect()
        {
            (_target as MonoBehaviour)?.StopAllCoroutines();
        }

        protected IEnumerator WaitForRecovery(int seconds)
        {
            yield return new WaitForSeconds(seconds);
            Recover();
        }
    }
}
