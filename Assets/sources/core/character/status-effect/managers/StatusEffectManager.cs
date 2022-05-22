using PataRoad.Core.Character.Equipments.Logic;
using System.Collections;
using UnityEngine;

namespace PataRoad.Core.Character
{
    public class StatusEffectManager : MonoBehaviour
    {
        protected IAttackable _target;
        public bool IsOnStatusEffect => CurrentStatusEffect != StatusEffectType.None;
        public bool IgnoreStatusEffect { get; set; }
        protected bool _isOnFire => CurrentStatusEffect == StatusEffectType.Fire;

        protected virtual Transform _transform => transform;

        private System.Collections.Generic.Dictionary<StatusEffectType, GameObject> _effectObjects;
        public virtual bool CanContinue => !IsOnStatusEffect && !_target.IsDead;

        private readonly UnityEngine.Events.UnityEvent<StatusEffectType> _onRecover
            = new UnityEngine.Events.UnityEvent<StatusEffectType>();
        internal bool IsBigTarget { get; set; }

        private UnityEngine.Events.UnityEvent<StatusEffectType> _onStatusEffect;
        public StatusEffectType CurrentStatusEffect { get; protected set; }

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
            InitEffects();
        }
        protected void InitEffects()
        {
            var effectInstantiator = FindObjectOfType<StatusEffectData>();
            if (effectInstantiator != null)
            {
                _effectObjects = IsBigTarget ? effectInstantiator.GetBossStatusEffectMap(_transform)
                    : effectInstantiator.GetStatusEffectMap(_transform);
            }
        }
        public void AddRecoverAction(UnityEngine.Events.UnityAction<StatusEffectType> action)
        {
            _onRecover.AddListener(action);
        }
        public void RemoveRecoverAction(UnityEngine.Events.UnityAction<StatusEffectType> action)
        {
            _onRecover.RemoveListener(action);
        }
        public virtual void SetFire(float time)
        {
            if (IsOnStatusEffect || time < 1 || IgnoreStatusEffect) return;
            OnStatusEffect?.Invoke(StatusEffectType.Fire);
            StopEverythingBeforeStatusEffect(StatusEffectType.Fire);

            StartCoroutine(FireDamage());
            OnFire();

            LoadEffectObject(StatusEffectType.Fire);
            CurrentStatusEffect = StatusEffectType.Fire;

            IEnumerator FireDamage()
            {
                while (time > 0)
                {
                    if (OnFireInterval())
                    {
                        yield return new WaitForEndOfFrame();
                        _target.Die();
                        break;
                    }
                    else yield return new WaitForSeconds(1);
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
        protected virtual bool OnFireInterval() =>
            DamageCalculator.DealDamageFromFireEffect(_target, gameObject, _transform);

        public virtual void SetIce(float time) => OnStatusEffect?.Invoke(StatusEffectType.Ice);
        public virtual void SetSleep(float time) => OnStatusEffect?.Invoke(StatusEffectType.Sleep);
        public virtual void SetStagger() => OnStatusEffect?.Invoke(StatusEffectType.Stagger);
        public virtual void SetKnockback() => OnStatusEffect?.Invoke(StatusEffectType.Knockback);
        public virtual void Tumble() => OnStatusEffect?.Invoke(StatusEffectType.Tumble);
        public virtual void TumbleAttack(bool hasDamage = false) { }

        public void RecoverAndIgnoreEffect()
        {
            Recover();
            IgnoreStatusEffect = true;
        }
        public void Recover() => Recover(true);
        public void RecoverWithoutInvoking() => Recover(false);
        protected void Recover(bool invokeOnRecover)
        {
            if (!IsOnStatusEffect || IgnoreStatusEffect) return;
            var effect = CurrentStatusEffect;
            StopAllCoroutines();
            if (_effectObjects != null && _effectObjects.ContainsKey(effect))
            {
                _effectObjects[effect].gameObject.SetActive(false);
            }
            CurrentStatusEffect = StatusEffectType.None;
            OnRecover(effect);

            if (invokeOnRecover && _onRecover != null) _onRecover.Invoke(effect);
        }

        /// <summary>
        /// Called when being recovered, before setting <see cref="IsOnStatusEffect"/> to <c>false</c>.
        /// </summary>
        protected virtual void OnRecover(StatusEffectType type) { }
        protected void LoadEffectObject(StatusEffectType type)
        {
            if (_effectObjects != null)
            {
                _effectObjects[type].gameObject.SetActive(true);
            }
        }
        protected virtual void StopEverythingBeforeStatusEffect(StatusEffectType type)
        {
            var mono = _target as MonoBehaviour;
            if (mono != null)
            {
                mono.StopAllCoroutines();
            }
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
