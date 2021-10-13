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
        /// <param name="times">How many times will do the attack. e.g. Yumipon 3 arrows in fever.</param>
        public abstract void Attack(int times = 1);
        protected void Init()
        {
            _holder = GetComponentInParent<Patapon.Patapon>();
        }
    }
}
