using PataRoad.Core.Character.Equipments;
using PataRoad.Core.Character.Equipments.Weapons;
using PataRoad.Core.Character.Patapons;
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

        /// <summary>
        /// Attack distance WITHOUT head size. Zero for melee expected. Some range units will add the distance by Tailwind.
        /// </summary>
        public virtual float AttackDistance { get; }
        /// <summary>
        /// Character size offest from center. Patapon head size, but if they have vehicle, it's depending on vehicle's head.
        /// </summary>

        public float CharacterSize { get; protected set; }

        protected AttackMoveController _attackController { get; private set; }
        public IAttackMoveData AttackMoveData { get; protected set; }

        public virtual Vector2 MovingDirection { get; }

        /// <summary>
        /// Root, which is parent of Patapon body. Default is empty, but Toripon has different root. Must add slash to end if it's not empty.
        /// </summary>
        public string RootName { get; protected set; } = "";

        internal string BodyName => RootName + "Patapon-body";

        public bool IsFlyingUnit { get; protected set; }

        internal EquipmentManager EquipmentManager { get; private set; }
        public StatusEffectManager StatusEffectManager { get; private set; }
        public Weapon Weapon => EquipmentManager.Weapon;
        private Rigidbody2D _rigidbody;
        [SerializeField]
        private UnityEngine.Events.UnityEvent _onAfterDeath;
        public void WeaponAttack(AttackCommandType type) => Weapon.Attack(type);

        public virtual CharacterSoundsCollection Sounds { get; protected set; }
        public bool IsDead { get; private set; }
        public bool IsMeleeUnit { get; protected set; }

        public UnityEvent<float> OnDamageTaken => null;

        protected virtual void Init()
        {
            Stat = _defaultStat;
            CurrentHitPoint = Stat.HitPoint;
            EquipmentManager = new EquipmentManager(gameObject);
            _rigidbody = GetComponent<Rigidbody2D>();
            CharAnimator = new CharacterAnimator(GetComponent<Animator>(), this);
            StatusEffectManager = gameObject.AddComponent<SmallCharacterStatusEffectManager>();
        }

        public virtual void StopAttacking()
        {
            StopWeaponAttacking();
            _attackController.StopAttack();
        }

        public virtual void Die()
        {
            MarkAsDead();
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

        /// <summary>
        /// Performs attack animation, applying attack seconds in stat.
        /// </summary>
        /// <param name="animationType">Animation name in animator.</param>
        /// <note>If this doesn't attack when Patapon is too fast, check if *AttackMultiplyer* is applied to the *Animation* in Animator.</note>
        protected void StartAttack(string animationType)
        {
            _attackController.StartAttack(animationType);
        }

        protected virtual void StopWeaponAttacking() => Weapon.StopAttacking();

        protected virtual AttackMoveController SetAttackMoveController()
        {
            _attackController = gameObject.AddComponent<AttackMoveController>();
            return _attackController;
        }
        protected AttackMoveController AddDefaultModelsToAttackMoveController()
        {
            if (_attackController == null) SetAttackMoveController();
            _attackController
                .AddModels(new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack", GetAttackMoveModel("attack") },
                    { "defend", GetAttackMoveModel("defend", AttackMoveType.Defend) },
                });
            return _attackController;
        }
        /// <summary>
        /// Get Attack move model based on Patapon default stats.
        /// </summary>
        /// <param name="animationType">Animation name in animator.</param>
        /// <param name="type">Telling attack movement type, if it's attack, defend or rush.</param>
        /// <param name="movingSpeed">Moving speed MULTIPLIER. It automatically multiplies to <see cref="Stat.MovementSpeed"/>.</param>
        /// <param name="attackSpeedMultiplier">Attack speed multiplier, default is 1. Yumipon fever attack is expected to 3.</param>
        /// <param name="attackDistance">Attack distance. default distance value is <see cref="AttackDistance"/>.</param>
        /// <returns>Attack Move Model for <see cref="AttackMoveController"/>.</returns>
        protected AttackMoveModel GetAttackMoveModel(string animationType, AttackMoveType type = AttackMoveType.Attack, float movingSpeed = 1, float attackSpeedMultiplier = 1, float attackDistance = -1)
        {
            movingSpeed *= Stat.MovementSpeed;
            return new AttackMoveModel(
                this,
                animationType,
                type,
                movingSpeed,
                attackSpeedMultiplier,
                attackDistance
                );
        }

        public abstract float GetAttackValueOffset();
        public abstract float GetDefenceValueOffset();

        public virtual void OnAttackHit(Vector2 point, int damage) => AttackMoveData.WasHitLastTime = true;
        public void OnAttackMiss(Vector2 point)
        {
            AttackMoveData.LastHit = point;
            AttackMoveData.WasHitLastTime = false;
        }

        public virtual void TakeDamage(int damage) => CurrentHitPoint -= damage;
        public void AddMass(float mass) => _rigidbody.mass += mass;
    }
}
