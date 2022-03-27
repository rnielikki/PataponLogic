using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    /// <summary>
    /// Represents any type of melee weapons.
    /// </summary>
    /// <remarks>Melee weapons do Slash, Stab or Crush, depends on weapon or user selection.</remarks>
    public abstract class MeleeWeapon : Weapon
    {
        protected BoxCollider2D _collider { get; private set; }
        protected override void LoadRenderersAndImage()
        {
            base.LoadRenderersAndImage();
            _collider = GetComponent<BoxCollider2D>();
        }
        public override void Attack(AttackCommandType attackCommandType)
        {
            _collider.enabled = true;
        }
        protected override void ReplaceImage(EquipmentData equipmentData)
        {
            base.ReplaceImage(equipmentData);
            SetColliderBoundingBox(_collider, equipmentData.Image);
        }

        public override void StopAttacking()
        {
            _collider.enabled = false;
            EndAttack();
        }
        protected virtual void EndAttack() { }
        /// <summary>
        /// Deals damage to the target when the collision and certain condition reached.
        /// </summary>
        /// <param name="collision">the collider from e.g. <see cref="OnTriggerEnter2D"/>.</param>
        protected void DealDamage(Collider2D collision) =>
            Logic.DamageCalculator.DealDamage(Holder, Holder.Stat, collision.gameObject, collision.ClosestPoint(transform.position));
    }
}
