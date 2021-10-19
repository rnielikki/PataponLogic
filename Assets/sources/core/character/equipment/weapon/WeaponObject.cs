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
        /// <summary>
        /// Load corresponding weapon instance resource from Resources/Characters/Equipments/PrefabBase.
        /// </summary>
        /// <param name="name">The name of instance (from the resource path).</param>
        /// <returns>The loaded game object from resource.</returns>
        protected GameObject GetWeaponInstance(string name = "WeaponInstance")
        {
            var obj = Resources.Load("Characters/Equipments/PrefabBase/" + name) as GameObject;
            obj.layer = gameObject.layer;
            return obj;
        }
    }
}
