using PataRoad.Core.Character.Class;
using PataRoad.Core.Character.Equipments.Weapons;
using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Character
{
    /// <summary>
    /// Represents small character, including Patapon and its enemy (Hazoron).
    /// </summary>
    public abstract class SmallCharacter : MonoBehaviour, ICharacter, Map.Weather.IWeatherReceiver
    {
        protected SmallCharacterData _data;
        public Stat Stat { get; protected set; }
        public Equipments.EquipmentManager EquipmentManager => _data.EquipmentManager;
        public ClassType Type => _data.Type;
        //----------------- needs only for mission fight
        /// <summary>
        /// Simple animator that moves pataponsa and not simple animator for attacking.
        /// </summary>
        public CharacterAnimator CharAnimator { get; protected set; }
        /// <summary>
        /// Current Hit point.
        /// <remarks>It shouldn't be bigger than <see cref="Stat.HitPoint"/> or smaller than 0. If this value is 0, it causes death.</remarks>
        /// </summary>
        public int CurrentHitPoint { get; protected set; }

        public DistanceCalculator DistanceCalculator { get; protected set; }
        public virtual float DefaultWorldPosition { get; protected set; }
        public DistanceManager DistanceManager { get; protected set; }

        /// <summary>
        /// Attack distance WITHOUT head size. Zero for melee expected. Some range units will add the distance by Tailwind.
        /// </summary>
        public virtual float AttackDistance { get; }
        /// <summary>
        /// Character size offest from center. Patapon head size, but if they have vehicle, it's depending on vehicle's head.
        /// </summary>
        public float CharacterSize { get; protected set; }

        public ClassData ClassData { get; protected set; }

        public virtual Vector2 MovingDirection { get; }

        public string RootName => ClassData.RootName;
        public const string CharacterBodyName = "Patapon-body";
        internal string BodyName => ClassData.RootName + CharacterBodyName;
        public Transform RootTransform { get; private set; }

        public bool IsFlyingUnit => ClassData.IsFlyingUnit;

        public StatusEffectManager StatusEffectManager { get; private set; }
        public Weapon Weapon => _data.EquipmentManager.Weapon;
        [SerializeField]
        protected UnityEvent _onAfterDeath;
        public UnityEvent OnAfterDeath => _onAfterDeath;
        public void WeaponAttack(AttackCommandType type) => Weapon.Attack(type);

        public virtual CharacterSoundsCollection Sounds { get; protected set; }
        public bool IsDead { get; protected set; }
        public bool IsMeleeUnit => ClassData.IsMeleeUnit;

        public UnityEvent<float> OnDamageTaken { get; set; } = null;

        // ----------- data from Patapon but for class data.
        /// <summary>
        /// Represents if PONCHAKA song is used before command (and in a row). This can be used for PONCHAKA~PONPON or PONCHAKA~CHAKACHAKA command.
        /// </summary>
        public bool Charged { get; protected set; }
        public bool OnFever { get; protected set; }

        public AttackType AttackType => Weapon.AttackType;
        public AttackTypeResistance AttackTypeResistance { get; private set; }
        public ElementalAttackType ElementalAttackType { get; set; }
        public abstract int AttackTypeIndex { get; }

        public float Sight => CharacterEnvironment.Sight + ClassData.AdditionalSight;

        public virtual bool IsFixedPosition { get; protected set; }

        public bool UseCenterAsAttackTarget => Weapon.IsTargetingCenter;

        public bool IsAttacking { get; protected set; }
        [SerializeField]
        private bool _ignoreTumble;
        public bool IgnoreTumble => _ignoreTumble;
        protected virtual void Init()
        {
            _data = GetComponent<SmallCharacterData>();
            _data.Init();
            ElementalAttackType = _data.ElementalAttackType;
            Stat = _data.Stat;
            CurrentHitPoint = _data.Stat.HitPoint;
            ClassData = ClassData.GetClassData(this, _data.Type);
            if (!string.IsNullOrEmpty(RootName))
            {
                RootTransform = transform.Find(RootName);
            }
            else
            {
                RootTransform = transform;
            }
            CharAnimator = new CharacterAnimator(GetComponent<Animator>(), this);
            StatusEffectManager = gameObject.AddComponent<SmallCharacterStatusEffectManager>();

            AttackTypeResistance = _data.AttackTypeResistance;
            InitDistanceFromHead();
        }
        /// <summary>
        /// Sets distance from calculated Patapon head. Don't use this if the Patapon uses any vehicle.
        /// </summary>
        protected void InitDistanceFromHead()
        {
            CharacterSize = transform.Find(BodyName + "/Face")
                .GetComponent<Collider2D>().bounds.size.x / 2 + 0.1f;
        }

        public virtual void StopAttacking(bool pause)
        {
            IsAttacking = false;
            StopWeaponAttacking();
            ClassData.StopAttack(pause);
        }
        public void TumbleStatusAttack() => StatusEffectManager.TumbleAttack();
        public virtual void Die() => Die(true);
        public void DieWithoutInvoking() => Die(false);
        protected void Die(bool invokeAfterDeath, bool animate = true)
        {
            if (IsDead) return;
            MarkAsDead();
            if (IsFlyingUnit)
            {
                CharAnimator.Animate("tori-fly-stop");
            }
            StatusEffectManager.RecoverAndIgnoreEffect();
            if (animate)
            {
                CharAnimator.PlayDyingAnimation();
                Destroy(gameObject, 1);
            }
            else
            {
                Destroy(gameObject);
            }
            AfterDie();
            if (invokeAfterDeath) _onAfterDeath?.Invoke();
        }
        protected void DestroyThis() => Destroy(gameObject);
        protected virtual void BeforeDie()
        {
            GameSound.SpeakManager.Current.Play(Sounds.OnDead);
        }
        protected virtual void AfterDie()
        {
            CurrentHitPoint = 0;
        }
        protected void MarkAsDead()
        {
            IsDead = true;
            BeforeDie();
            StopAttacking(false);
        }
        protected virtual void StopWeaponAttacking() => Weapon.StopAttacking();
        public abstract float GetAttackValueOffset();
        public abstract float GetDefenceValueOffset();

        public virtual void OnAttackHit(Vector2 point, int damage) => ClassData.AttackMoveData.WasHitLastTime = true;
        public void OnAttackMiss(Vector2 point)
        {
            ClassData.AttackMoveData.LastHit = point;
            ClassData.AttackMoveData.WasHitLastTime = false;
        }
        internal void SetMaximumHitPoint(int point)
        {
            Stat.HitPoint = CurrentHitPoint = point;
        }
        public virtual bool TakeDamage(int damage)
        {
            CurrentHitPoint -= damage;
            return true;
        }

        public void ReceiveWeather(Map.Weather.WeatherType weatherType)
        {
            if (weatherType != Map.Weather.WeatherType.Snow) return;
            if (!StatusEffectManager.IsOnStatusEffect
                &&
                Common.Utils.RandomByProbability((1 - Stat.IceResistance) * 0.02f))
            {
                StatusEffectManager.SetIce(4);
            }
        }
    }
}
