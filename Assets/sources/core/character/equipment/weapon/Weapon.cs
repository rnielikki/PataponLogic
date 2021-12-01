using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    public abstract class Weapon : Equipment
    {
        public virtual float MinAttackDistance { get; }

        /// <summary>
        /// Additional force from environment from WIND.
        /// </summary>
        public virtual float WindAttackDistanceOffset { get; }
        /// <summary>
        /// Sprite of THROWABLE object, like arrows or spears.
        /// </summary>
        public Sprite ThrowableWeaponSprite { get; protected set; }

        protected override EquipmentType _type => EquipmentType.Weapon;

        [SerializeField]
        private AttackType _attackType;
        public AttackType AttackType { get; internal set; }
        private void Start()
        {
            Load();
        }

        protected virtual void Init()
        {
            AttackType = _attackType;
            Holder = GetComponentInParent<SmallCharacter>();
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
