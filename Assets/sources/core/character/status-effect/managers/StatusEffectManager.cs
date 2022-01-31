using PataRoad.Core.Character.Equipments.Logic;
using System.Collections;
using UnityEngine;

namespace PataRoad.Core.Character
{
    public class StatusEffectManager : MonoBehaviour
    {
        protected IAttackable _target;
        public bool IsOnStatusEffect { get; protected set; }
        public bool IgnoreStatusEffect { get; set; }
        protected bool _isOnFire;

        protected Transform _transform;

        private GameObject _effectObject;
        private StatusEffectData _effectInstantiator;
        public virtual bool CanContinue => !IsOnStatusEffect && !_target.IsDead;

        private UnityEngine.Events.UnityEvent _onRecover = new UnityEngine.Events.UnityEvent();
        internal bool IsBigTarget { get; set; }

        private UnityEngine.Events.UnityEvent<StatusEffectType> _onStatusEffect;
        public UnityEngine.Events.UnityEvent<StatusEffectType> OnStatusEffect
        {
            get
            {
                if (_onStatusEffect == null) _onStatusEffect = new UnityEngine.Events.UnityEvent<StatusEffectType>();
                return _onStatusEffect;
            }
        }

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
        public void AddRecoverAction(UnityEngine.Events.UnityAction action)
        {
            _onRecover.AddListener(action);
        }
        public virtual void SetFire(float time)
        {
            if (IsOnStatusEffect || time < 1 || IgnoreStatusEffect) return;
            _isOnFire = true;
            OnStatusEffect?.Invoke(StatusEffectType.Fire);
            StopEverythingBeforeStatusEffect();

            StartCoroutine(FireDamage());
            OnFire();

            LoadEffectObject(StatusEffectType.Fire);
            IsOnStatusEffect = true;

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
        /// Called when fire effect starts, before setting <see cref="IsOnStatusEffect"/> to <c>true</c>.
        /// </summary>
        protected virtual void OnFire() { }
        /// <summary>
        /// Every time on fire interval. Usually it's "when damage taken from fire" (which is impelemented in base)
        /// </summary>
        protected virtual void OnFireInterval()
        {
            DamageCalculator.DealDamageFromFireEffect(_target, gameObject, _transform);
        }
        public virtual void SetIce(float time) => OnStatusEffect?.Invoke(StatusEffectType.Ice);
        public virtual void SetSleep(float time) => OnStatusEffect?.Invoke(StatusEffectType.Sleep);
        public virtual void SetStagger() => OnStatusEffect?.Invoke(StatusEffectType.Stagger);
        public virtual void SetKnockback() => OnStatusEffect?.Invoke(StatusEffectType.Knockback);
        public virtual void Tumble() => OnStatusEffect?.Invoke(StatusEffectType.Tumble);
        public virtual void TumbleAttack() { }

        public void RecoverAndIgnoreEffect()
        {
            Recover();
            IgnoreStatusEffect = true;
        }
        public void Recover()
        {
            if (!IsOnStatusEffect || IgnoreStatusEffect) return;
            StopAllCoroutines();
            _isOnFire = false;
            if (_effectObject != null)
            {
                Destroy(_effectObject);
            }
            IsOnStatusEffect = false;
            OnRecover();

            if (_onRecover != null) _onRecover.Invoke();
        }

        /// <summary>
        /// Called when being recovered, before setting <see cref="IsOnStatusEffect"/> to <c>false</c>.
        /// </summary>
        protected virtual void OnRecover() { }
        protected void LoadEffectObject(StatusEffectType type)
        {
            //can be resized if big, I guess
            _effectObject = _effectInstantiator.AttachEffect(type, _transform, IsBigTarget);
        }
        protected virtual void StopEverythingBeforeStatusEffect()
        {
            (_target as MonoBehaviour)?.StopAllCoroutines();
        }

        protected IEnumerator WaitForRecovery(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Recover();
        }
        private void OnDestroy()
        {
            _onStatusEffect?.RemoveAllListeners();
        }
    }
}
