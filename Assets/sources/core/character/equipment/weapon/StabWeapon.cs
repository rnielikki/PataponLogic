using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    /// <summary>
    /// Stab: Deals damage when it's staying in trigger.
    /// </summary>
    public class StabWeapon : MeleeWeapon
    {
        private readonly System.Collections.Generic.Dictionary<Collider2D, Coroutine> _colliderCoroutineMap
            = new System.Collections.Generic.Dictionary<Collider2D, Coroutine>();
        private float _damageSeconds;
        private void Start()
        {
            Init();
            if (Holder != null) _damageSeconds = Holder.Stat.AttackSeconds / 4;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_colliderCoroutineMap.ContainsKey(collision)) _colliderCoroutineMap.Add(
                collision,
                StartCoroutine(DamageOnTime(collision)));
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_colliderCoroutineMap.TryGetValue(collision, out Coroutine coroutine))
            {
                if (coroutine != null) StopCoroutine(coroutine);
                _colliderCoroutineMap.Remove(collision);
            }
        }
        protected override void EndAttack()
        {
            StopAllCoroutines();
            _colliderCoroutineMap.Clear();
        }
        private System.Collections.IEnumerator DamageOnTime(Collider2D collision)
        {
            while (collision != null && collision.enabled)
            {
                DealDamage(collision);
                yield return new WaitForSeconds(_damageSeconds);
            }
        }
    }
}
