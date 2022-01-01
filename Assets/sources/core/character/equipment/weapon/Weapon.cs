using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    public abstract class Weapon : Equipment
    {
        /// <summary>
        /// Sprite of THROWABLE object, like arrows or spears.
        /// </summary>
        public Sprite ThrowableWeaponSprite { get; protected set; }
        public AttackCommandType LastAttackCommandType { get; private set; }
        protected Vector2 _initialVelocity { get; private set; }

        protected override EquipmentType _type => EquipmentType.Weapon;
        protected virtual float _throwMass => Mass;
        protected Color _color { get; private set; } = Color.white;

        [SerializeField]
        private AttackType _attackType;
        public AttackType AttackType { get; internal set; }
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
        protected GameObject GetWeaponInstance(string name = "WeaponInstance")
        {
            return Resources.Load("Characters/Equipments/PrefabBase/" + name) as GameObject;
        }
        internal virtual void SetLastAttackCommandType(AttackCommandType attackCommandType)
        {
            LastAttackCommandType = attackCommandType;
        }
        protected void SetInitialVelocity(float force, float angle, Vector2 additionalDir = default)
        {
            _initialVelocity = Time.fixedDeltaTime * force * (new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) + additionalDir);
        }
        /// <summary>
        /// Gets throwing range attack distance. It also considers wind, but doesn't consider y axis. Use <see cref="AdjustAttackDistanceByYPosition(float, float)"/> for late adjustment.
        /// </summary>
        /// <returns>x distance for range attack.</returns>
        protected float GetThrowingAttackDistance()
        {
            return (2 * _initialVelocity.x * _initialVelocity.y) / -Physics2D.gravity.y
                + 2 * Map.Weather.WeatherInfo.Current.Wind.Magnitude * Mathf.Pow(_initialVelocity.y, 2) / Mathf.Pow(Physics2D.gravity.y, 2);
        }
        // Parabola approximation
        public virtual float AdjustAttackDistanceByYPosition(float attackDistance, float yDistance) => attackDistance;
        protected float AdjustThrowingAttackDistanceByYPosition(float attackDistance, float yDistance)
        {
            if (_initialVelocity.x == 0) return 0; //No zero division
            var velocityRate = _initialVelocity.y / _initialVelocity.x;
            return Mathf.Sqrt((yDistance + 0.25f * velocityRate * Mathf.Pow(attackDistance, 2)) / velocityRate) + 0.5f * attackDistance;
        }
        internal virtual void Colorize(Color color)
        {
            if (_spriteRenderers == null) LoadRenderersAndImage();
            foreach (var renderer in _spriteRenderers)
            {
                renderer.color = color;
            }
            _color = color;
        }
    }
}
