using PataRoad.Core.Character.Class;
using PataRoad.Core.Character.Equipments.Weapons;
using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Character
{
    /// <summary>
    /// Represents small character, including Patapon and its enemy (Hazoron).
    /// </summary>
    public abstract class SmallCharacter : MonoBehaviour, ICharacter
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
        internal string BodyName => ClassData.RootName + "Patapon-body";
        public Transform RootTransform { get; private set; }

        public bool IsFlyingUnit => ClassData.IsFlyingUnit;

        public StatusEffectManager StatusEffectManager { get; private set; }
        public Weapon Weapon => _data.EquipmentManager.Weapon;
        [SerializeField]
        private UnityEvent _onAfterDeath;
        public UnityEvent OnAfterDeath => _onAfterDeath;
        public void WeaponAttack(AttackCommandType type) => Weapon.Attack(type);

        public virtual CharacterSoundsCollection Sounds { get; protected set; }
        public bool IsDead { get; private set; }
        public bool IsMeleeUnit => ClassData.IsMeleeUnit;

        public UnityEvent<float> OnDamageTaken => null;

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

        public float Sight => CharacterEnvironment.Sight;

        protected void Init()
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
                .GetComponent<CircleCollider2D>().radius + 0.1f;
        }

        public virtual void StopAttacking(bool pause)
        {
            StopWeaponAttacking();
            ClassData.StopAttack(pause);
        }

        public virtual void Die()
        {
            if (IsDead) return;
            MarkAsDead();
            if (IsFlyingUnit)
            {
                CharAnimator.AnimateFrom("tori-fly-stop");
            }
            StartCoroutine(WaitUntilDie());
            StatusEffectManager.RecoverAndIgnoreEffect();
            System.Collections.IEnumerator WaitUntilDie()
            {
                CharAnimator.PlayDyingAnimation();
                yield return new WaitForSeconds(1);
                AfterDie();
                _onAfterDeath?.Invoke();
                Destroy(gameObject);
            }
        }
        protected virtual void BeforeDie() { }
        protected virtual void AfterDie() { }
        protected void MarkAsDead()
        {
            IsDead = true;
            CurrentHitPoint = 0;
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

        public virtual void TakeDamage(int damage) => CurrentHitPoint -= damage;
    }
}
