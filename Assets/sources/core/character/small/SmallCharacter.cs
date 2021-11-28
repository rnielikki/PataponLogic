using PataRoad.Core.Character.Class;
using PataRoad.Core.Character.Equipments;
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
        /// <summary>
        /// Current Stat.
        /// </summary>
        public virtual Stat Stat { get; protected set; }

        /// <summary>
        /// Default stat that a bit varies for each class.
        /// </summary>
        [SerializeReference]
        protected Stat _defaultStat = new Stat();

        /// <summary>
        /// Current Hit point.
        /// <remarks>It shouldn't be bigger than <see cref="Stat.HitPoint"/> or smaller than 0. If this value is 0, it causes death.</remarks>
        /// </summary>
        public int CurrentHitPoint { get; protected set; }

        /// <summary>
        /// Class (e.g. Yaripon, Tatepon, Yumipon...) of the Patapon.
        /// </summary>
        public ClassType Class { get; protected set; }

        /// <summary>
        /// Simple animator that moves patapons.
        /// </summary>
        public CharacterAnimator CharAnimator { get; protected set; }

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

        public bool IsFlyingUnit => ClassData.IsFlyingUnit;

        internal EquipmentManager EquipmentManager { get; private set; }
        public StatusEffectManager StatusEffectManager { get; private set; }
        public Weapon Weapon => EquipmentManager.Weapon;
        private Rigidbody2D _rigidbody;
        [SerializeField]
        private UnityEvent _onAfterDeath;
        public UnityEvent OnAfterDeath => _onAfterDeath;
        public void WeaponAttack(AttackCommandType type) => Weapon.Attack(type);

        public virtual CharacterSoundsCollection Sounds { get; protected set; }
        public bool IsDead { get; private set; }
        public bool IsMeleeUnit => ClassData.IsMeleeUnit;

        public UnityEvent<float> OnDamageTaken => null;
        [SerializeField]
        protected ClassType _type;
        public ClassType Type => _type;

        // ----------- data from Patapon but for class data.
        /// <summary>
        /// Represents if PONCHAKA song is used before command (and in a row). This can be used for PONCHAKA~PONPON or PONCHAKA~CHAKACHAKA command.
        /// </summary>
        public bool Charged { get; protected set; }
        public bool OnFever { get; protected set; }

        protected void Init()
        {
            Stat = _defaultStat;
            CurrentHitPoint = Stat.HitPoint;
            ClassData = ClassData.GetClassData(this, _type);
            EquipmentManager = new EquipmentManager(gameObject);

            _rigidbody = GetComponent<Rigidbody2D>();

            CharAnimator = new CharacterAnimator(GetComponent<Animator>(), this);
            StatusEffectManager = gameObject.AddComponent<SmallCharacterStatusEffectManager>();
        }

        public virtual void StopAttacking()
        {
            StopWeaponAttacking();
            ClassData.StopAttack();
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
            System.Collections.IEnumerator WaitUntilDie()
            {
                CharAnimator.PlayDyingAnimation();
                yield return new WaitForSeconds(1);
                AfterDie();
                _onAfterDeath.Invoke();
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
            StopAttacking();
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
        public void AddMass(float mass) => _rigidbody.mass += mass;
    }
}
