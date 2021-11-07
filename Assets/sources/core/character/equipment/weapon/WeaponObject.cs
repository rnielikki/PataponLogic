using UnityEngine;

namespace PataRoad.Core.Character.Equipment.Weapon
{
    public abstract class WeaponObject : MonoBehaviour
    {
        public Weapon Data { get; protected set; }
        public ICharacter Holder { get; protected set; }

        protected virtual float _minAttackDistance { get; set; }
        /// <summary>
        /// Attack distance of the wepaon.
        /// </summary>
        public virtual float AttackDistance => _minAttackDistance + AttackDistanceOffset;
        /// <summary>
        /// Additional force from environment, like wind.
        /// </summary>
        public virtual float AttackDistanceOffset { get; protected set; }
        /// <summary>
        /// Sprite of THROWABLE object, like arrows or spears.
        /// </summary>
        public Sprite ThrowableWeaponSprite { get; protected set; }

        protected virtual void Init()
        {
            Holder = GetComponentInParent<ICharacter>();
            ThrowableWeaponSprite = GetThrowableWeaponSprite();
        }
        /// <summary>
        /// Perform weapon specific attack.
        /// </summary>
        public abstract void Attack(AttackCommandType attackCommandType);
        /// <summary>
        /// Stops attacking. This is meaningful for melee units.
        /// </summary>
        public virtual void StopAttacking() { }
        /// <summary>
        /// Sets throwable object sprite, like arrows or spears.
        /// </summary>
        protected virtual Sprite GetThrowableWeaponSprite() => GetComponent<SpriteRenderer>().sprite;
        /// <summary>
        /// Load corresponding weapon instance resource from Resources/Characters/Equipments/PrefabBase.
        /// </summary>
        /// <param name="name">The name of instance (from the resource path).</param>
        /// <returns>The loaded game object from resource.</returns>
        protected GameObject GetWeaponInstance(string name = "WeaponInstance")
        {
            return Resources.Load("Characters/Equipments/PrefabBase/" + name) as GameObject;
        }
    }
}
