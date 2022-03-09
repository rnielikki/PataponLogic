using PataRoad.Commom;
using PataRoad.Core.Items;
using UnityEngine;
using UnityEngine.Pool;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    public abstract class Weapon : Equipment
    {
        /// <summary>
        /// Gameobject containing pooled gameobjects
        /// </summary>
        protected IObjectPool<GameObject> _objectPool;
        protected string resourcePath = "Characters/Equipments/PrefabBase/WeaponInstance";
        protected int poolInitialSize = 30;
        protected int poolMaxSize = 100;
        /// <summary>
        /// Sprite of THROWABLE object, like arrows or spears.
        /// </summary>
        public Sprite ThrowableWeaponSprite { get; protected set; }
        public AttackCommandType LastAttackCommandType { get; private set; }
        protected Vector2 _initialVelocity { get; private set; }

        protected override EquipmentType _type => EquipmentType.Weapon;
        protected virtual float _throwMass => Mass;
        protected Material _material { get; set; }

        [SerializeField]
        private AttackType _attackType;
        public AttackType AttackType { get; internal set; }
        public virtual bool IsTargetingCenter { get; }

        private void Start()
        {
            Load();
        }

        protected virtual void Init()
        {
            Load();
            AttackType = _attackType;
            Holder = GetComponentInParent<SmallCharacter>();
            ThrowableWeaponSprite = GetThrowableWeaponSprite();
            var poolObject = GameObject.Find(nameof(GameObjectPool));
            if (poolObject != null)
            {
                _objectPool = poolObject
                .GetComponent<GameObjectPool>()
                .GetPool(resourcePath, poolInitialSize, poolMaxSize);
            }
        }
        public virtual float GetAttackDistance() => 0;
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
        protected GameObject GetWeaponInstance(string name = null)
        {
            if (name == null) return WeaponInstance.GetResource();
            return Resources.Load("Characters/Equipments/PrefabBase/" + name) as GameObject;
        }
        /// <summary>
        /// "Prewarms" before attacking. Useful for throwing weapons' attack distance calculation.
        /// </summary>
        /// <param name="attackCommandType">The attack command type that may determine e.g. initial velocity.</param>
        internal virtual void SetLastAttackCommandType(AttackCommandType attackCommandType)
        {
            LastAttackCommandType = attackCommandType;
        }
        protected void SetInitialVelocity(float force, float angle)
        {
            _initialVelocity = Time.fixedDeltaTime * force
                * (new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)));
        }
        /// <summary>
        /// Gets throwing range attack distance. It also considers wind, but doesn't consider y axis. Use <see cref="AdjustAttackDistanceByYPosition(float, float)"/> for late adjustment.
        /// </summary>
        /// <remarks>This is calculated from physics theory for grounded attacker.</remarks>
        /// <returns>x distance for range attack.</returns>
        protected float GetThrowingAttackDistance()
        {
            return ((2 * _initialVelocity.x * _initialVelocity.y) / -Physics2D.gravity.y)
                + (2 * Map.Weather.WeatherInfo.Current.Wind.Magnitude * Mathf.Pow(_initialVelocity.y, 2)
                    / Mathf.Pow(Physics2D.gravity.y, 2));
        }
        // Parabola approximation
        public virtual float AdjustAttackDistanceByYPosition(float attackDistance, float yDistance) => attackDistance;
        protected float AdjustThrowingAttackDistanceByYPosition(float attackDistance, float yDistance)
        {
            if (_initialVelocity.x == 0) return 0; //No zero division
            var velocityRate = _initialVelocity.y / _initialVelocity.x;
            var yDiff = yDistance - Holder.RootTransform.position.y;
            return Mathf.Max(Mathf.Sqrt((yDiff + (0.25f * velocityRate * Mathf.Pow(attackDistance, 2))) / velocityRate)
                + (0.5f * attackDistance)
                - (Map.Weather.WeatherInfo.Current.Wind.Magnitude * Mathf.Clamp01(yDistance / CharacterEnvironment.MaxYToScan)), 0);
        }
        internal override void ReplaceEqupiment(EquipmentData equipmentData, Stat stat)
        {
            base.ReplaceEqupiment(equipmentData, stat);
            var gem = HolderData.EquipmentManager?.ElementGem;
            if (gem != null && gem.CurrentData != null)
            {
                _material = (gem.CurrentData as GemData).WeaponMaterial;
            }
        }
        protected override void LoadRenderersAndImage()
        {
            base.LoadRenderersAndImage();
            _material = _spriteRenderers[0].material;
        }
        internal virtual void Colorize(Material weaponMaterial)
        {
            if (_spriteRenderers == null) LoadRenderersAndImage();
            foreach (var renderer in _spriteRenderers)
            {
                renderer.material = weaponMaterial;
            }
            _material = weaponMaterial;
        }
    }
}
