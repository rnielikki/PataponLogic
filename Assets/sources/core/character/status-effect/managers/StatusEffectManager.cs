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
        public void SetFire(int time)
        {
            if (OnStatusEffect || time < 1 || IgnoreStatusEffect) return;
            StartStatusEffect();
            _onFire = true;

            StartCoroutine(FireDamage());
            OnFire();

            LoadEffectObject(StatusEffectType.Fire);
            OnStatusEffect = true;

            IEnumerator FireDamage()
            {
                int timeSum = 0;
                while (timeSum < time)
                {
                    DamageCalculator.DealDamageFromFireEffect(_target, gameObject, _transform);
                    yield return new WaitForSeconds(1);
                    timeSum++;
                }
                Recover();
            }
        }
        protected virtual void OnFire() { }
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
