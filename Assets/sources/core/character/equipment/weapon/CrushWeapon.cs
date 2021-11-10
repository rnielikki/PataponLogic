using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    /// <summary>
    /// Crush: Deals damage only once per attack (per collider).
    /// </summary>
    public class CrushWeapon : MeleeWeapon
    {
        private readonly System.Collections.Generic.List<Collider2D> _colliders = new System.Collections.Generic.List<Collider2D>();
        private void Start()
        {
            Init();
        }
        protected override void EndAttack()
        {
            _colliders.Clear();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_colliders.Contains(collision))
            {
                DealDamage(collision);
                _colliders.Add(collision);
            }
        }
    }
}
