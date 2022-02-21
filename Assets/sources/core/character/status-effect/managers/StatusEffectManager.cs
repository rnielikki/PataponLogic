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

        protected Transform _transform;

        private GameObject _effectObject;
        private StatusEffectData _effectInstantiator;
        public virtual bool CanContinue => !IsOnStatusEffect && !_target.IsDead;

        private readonly UnityEngine.Events.UnityEvent _onRecover = new UnityEngine.Events.UnityEvent();
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
        protected void Recover(bool invokeOnRecover)
        {
            if (!IsOnStatusEffect || IgnoreStatusEffect) return;
            StopAllCoroutines();
            if (_effectObject != null)
            {
                Destroy(_effectObject);
            }
            CurrentStatusEffect = StatusEffectType.None;
            OnRecover();

            if (invokeOnRecover && _onRecover != null) _onRecover.Invoke();
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
        protected virtual void StopEverythingBeforeStatusEffect(StatusEffectType type)
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
