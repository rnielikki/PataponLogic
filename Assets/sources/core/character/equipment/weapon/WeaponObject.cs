using UnityEngine;

namespace Core.Character.Equipment.Weapon
{
    public abstract class WeaponObject : MonoBehaviour
    {
        public Weapon Data { get; protected set; }
        protected Patapon.Patapon _holder;

        /// <summary>
        /// Perform weapon specific attack.
        /// </summary>
        public abstract void Attack(AttackType attackType);
        protected void Init()
        {
            _holder = GetComponentInParent<Patapon.Patapon>();
        }
    }
}
