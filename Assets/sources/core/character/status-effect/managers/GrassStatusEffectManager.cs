using UnityEngine;
using System.Linq;

namespace PataRoad.Core.Character
{
    class GrassStatusEffectManager : StatusEffectManager
    {
        float _totalTime;
        private Collider2D _collider;
        private ContactFilter2D _filter;

        /// <summary>
        /// Set fire for grass. Unlike others, it just adds ignition time when ignited on fire.
        /// </summary>
        /// <param name="time">how much time will it on fire (or time to being added on fire).</param>
        private void Start()
        {
            _collider = ((Grass)_target).Collider;

            _filter = new ContactFilter2D();
            _filter.SetLayerMask(LayerMask.GetMask("grass", "structures", "bosses", "hazorons", "patapons"));
            _filter.useTriggers = true;
        }
        public override void SetFire(float time)
        {
            if (time < 1) return;
            else if (IsOnStatusEffect)
            {
                _totalTime += time;
                return;
            }
            _totalTime = time;
            StopEverythingBeforeStatusEffect(StatusEffectType.Fire);
            OnStatusEffect?.Invoke(StatusEffectType.Fire);

            StartCoroutine(FireDamage());

            LoadEffectObject(StatusEffectType.Fire);
            CurrentStatusEffect = StatusEffectType.Fire;

            SpreadFire(time / 2);

            System.Collections.IEnumerator FireDamage()
            {
                while (_totalTime > 0)
                {
                    Equipments.Logic.DamageCalculator.DealDamageFromFireEffect(_target, gameObject, _transform, false);
                    yield return new WaitForSeconds(1);
                    _totalTime--;
                }
                Recover();
            }
        }
        private void SpreadFire(float time)
        {
            if (time < 1) return;
            System.Collections.Generic.List<Collider2D> others = new System.Collections.Generic.List<Collider2D>();
            _collider.OverlapCollider(_filter, others);
            var neighbours = others
                .Select(collider => collider.GetComponentInParent<IAttackable>())
                .ToArray();
            foreach (var neighbour in neighbours)
            {
                if (!neighbour.StatusEffectManager.IsOnStatusEffect)
                {
                    neighbour.StatusEffectManager.SetFire(
                        Equipments.Logic.DamageCalculator.GetFireDuration(_target.Stat, neighbour.Stat, time)
                    );
                }
            }
        }
    }
}
