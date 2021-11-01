using Core.Character.Equipment;
using Core.Character.Equipment.Weapon;
using Core.Character.Patapon.Animation;
using Core.Rhythm.Command;
using UnityEngine;

namespace Core.Character.Patapon
{
    /// <summary>
    /// Represents any one Patapon. Classes will inherited from this class.
    /// </summary>
    public abstract class Patapon : MonoBehaviour, ICharacter
    {
        /// <summary>
        /// Helm that the Patapon is currently using.
        /// <note>This will do nothing unless if <see cref="Rarepon"/> is set to normal.</note>
        /// </summary>
        public IEquipment Helm { get; protected set; }
        /// <summary>
        /// Weapon (e.g. spear, swrod...) that the Patapon uses.
        /// </summary>
        public WeaponObject Weapon { get; protected set; }
        /// <summary>
        /// Protector (e.g. horse, shoes, shield, shoulder...) that Patapon uses.
        /// <note>This is useless for specific classes, like Yaripon or Yumipon.</note>
        /// </summary>
        public IEquipment Protector { get; protected set; }
        /// <summary>
        /// Current Stat that a Patapon has. Any effect can modify it.
        /// </summary>
        public Stat Stat { get; protected set; }
        /// <summary>
        /// Default stat that a bit varies for each class.
        /// </summary>
        protected abstract Stat DefaultStat { get; }
        /// <summary>
        /// Current Hit point.
        /// <remarks>It shouldn't be bigger than <see cref="Stat.HitPoint"/> or smaller than 0. If this value is 0, it causes death.</remarks>
        /// </summary>
        public int CurrentHitPoint { get; set; }
        /// <summary>
        /// Class (e.g. Yaripon, Tatepon, Yumipon...) of the Patapon.
        /// </summary>
        public ClassType Class { get; protected set; }
        /// <summary>
        /// Rarepon type of this Patapon.
        /// </summary>
        public Rarepon Rarepon { get; protected set; }

        /// <summary>
        /// Represents if PONCHAKA song is used before command (and in a row). This can be used for PONCHAKA~PONPON or PONCHAKA~CHAKACHAKA command.
        /// </summary>
        protected bool _charged { get; private set; }

        /// <summary>
        /// Simple animator that moves patapons.
        /// </summary>
        public PataponAnimator PataponAnimator { get; private set; }

        /// <summary>
        /// Sets Patapon distance.
        /// </summary>
        public PataponDistance PataponDistance { get; private set; }

        /// <summary>
        /// Current Patapon Index, from first of the line to the end of the line. Index starts from 0.
        /// </summary>
        public int Index { get; internal set; }
        /// <summary>
        /// Current Patapon Index IN <see cref="PataponGroup"/>, from first of the line to the end of the line. Index starts from 0.
        /// </summary>
        public int IndexInGroup { get; internal set; }

        protected CommandSong _lastSong;
        private float _lastPerfectionPercent;

        public AttackType AttackType { get; protected set; }

        /// <summary>
        /// Attack distance WITHOUT head size. Zero for melee expected.
        /// </summary>
        public float AttackDistance { get; protected set; }
        /// <summary>
        /// Patapon size offest from center. Patapon head size, but if they have vehicle, it's depending on vehicle's head.
        /// </summary>
        public float PataponSize { get; protected set; }
        protected AttackMoveController _attackController { get; private set; }
        public float AttackDistanceWithOffset => AttackDistance + PataponSize;

        /// <summary>
        /// Remember call this on Awake() in inherited class
        /// </summary>
        protected void Init()
        {
            PataponDistance = GetComponent<PataponDistance>();
            PataponAnimator = new PataponAnimator(GetComponent<Animator>());
            Stat = DefaultStat;
            Weapon = GetComponentInChildren<WeaponObject>();
        }
        protected AttackMoveController SetAttackMoveController()
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
        /// Sets distance from calculated Patapon head. Don't use this if the Patapon uses any vehicle.
        /// </summary>
        /// <param name="attackDistance">Attack distance, without considering head size.</param>
        protected void InitDistanceFromHead(float attackDistance)
        {
            AttackDistance = attackDistance;
            PataponSize = transform.Find("Patapon-body/Face").GetComponent<CircleCollider2D>().radius + 0.1f;
            PataponDistance.InitDistance(
                attackDistance,
                PataponSize
            );
        }

        public void MoveOnDrum(string drumName)
        {
            StopAttacking();
            PataponAnimator.Animate(drumName);
            PataponDistance.StopMoving();
        }

        /// <summary>
        /// Recieves command song and starts corresponding moving.
        /// </summary>
        /// <param name="song">The command song, which determines what the patapon will act.</param>
        public virtual void Act(RhythmCommandModel model)
        {
            var song = model.Song;
            var isFever = model.ComboType == ComboStatus.Fever;
            if (_lastSong != song) PataponAnimator.ClearLateAnimation();
            _lastSong = song;
            _lastPerfectionPercent = model.Percentage;
            switch (song)
            {
                case CommandSong.Patapata:
                    Walk();
                    break;
                case CommandSong.Ponpon:
                    Attack(isFever);
                    break;
                case CommandSong.Chakachaka:
                    Defend(isFever);
                    break;
                case CommandSong.Ponpata:
                    Dodge();
                    break;
                case CommandSong.Ponchaka:
                    Charge();
                    break;
                case CommandSong.Dondon:
                    Jump();
                    break;
                case CommandSong.Donchaka:
                    Party();
                    break;
                case CommandSong.Patachaka:
                    PataponAnimator.Animate("walk");
                    PataponDistance.MoveToInitialPlace(Stat.MovementSpeed * 2);
                    break;
            }
            _charged = song == CommandSong.Ponchaka; //Removes charged status if it isn't charging command
        }
        public void DoMisisonCompleteGesture()
        {
            PataponAnimator.AnimateWithoutNormalizing("party");
        }

        /// <summary>
        /// Going back to Idle status. This also means ALL FEVER/COMMAND is CANCELED. also removes PONCHAKA <see cref="_charged"/> status.
        /// </summary>
        public virtual void PlayIdle()
        {
            _charged = false;
            StopAttacking();
            PataponAnimator.Animate("Idle");
            PataponDistance.MoveToInitialPlace(Stat.MovementSpeed);
        }
        /// <summary>
        /// PATAPATA Input
        /// </summary>
        void Walk()
        {
            PataponAnimator.Animate("walk");
            PataponDistance.MoveToInitialPlace(Stat.MovementSpeed);
        }
        /// <summary>
        /// PONPON Input
        /// </summary>
        protected virtual void Attack(bool isFever)
        {
            StartAttack("attack");
        }
        /// <summary>
        /// CHAKACHAKA Input
        /// </summary>
        protected virtual void Defend(bool isFever)
        {
            StartAttack("defend");
        }
        /// <summary>
        /// PONPATA Input
        /// </summary>
        protected virtual void Dodge()
        {
            PataponAnimator.Animate("dodge");
            PataponDistance.MoveBack(Stat.MovementSpeed * 1.5f);
        }
        /// <summary>
        /// PONCHAKA Input
        /// </summary>
        protected virtual void Charge()
        {
            PataponAnimator.Animate("charge");
            PataponDistance.MoveToInitialPlace(Stat.MovementSpeed);
        }

        /// <summary>
        /// DONDON Input
        /// </summary>
        protected virtual void Jump()
        {
            PataponAnimator.Animate("jump");
            PataponDistance.StopMoving();
        }

        /// <summary>
        /// DONCHAKA Input
        /// </summary>
        protected virtual void Party()
        {
            PataponAnimator.Animate("party");
            PataponDistance.MoveToInitialPlace(Stat.MovementSpeed);
        }
        public void WeaponAttack(AttackCommandType type) => Weapon.Attack(type);

        protected virtual void StopAttacking()
        {
            StopWeaponAttacking();
            _attackController.StopAttack();
        }
        protected virtual void StopWeaponAttacking() => Weapon.StopAttacking();

        /// <summary>
        /// Performs attack animation, applying attack seconds in stat.
        /// </summary>
        /// <param name="animationType">Animation name in animator.</param>
        /// <note>If this doesn't attack when Patapon is too fast, check if *AttackMultiplyer* is applied to the *Animation* in Animator.</note>
        protected void StartAttack(string animationType)
        {
            _attackController.StartAttack(animationType);
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
        protected AttackMoveModel GetAttackMoveModel(string animationType, AttackMoveType type = AttackMoveType.Attack, float movingSpeed = 1, float attackSpeedMultiplier = 1, float attackDistance = -100)
        {
            if (attackDistance < -10) attackDistance = AttackDistance;
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

        public void TakeDamage(int value)
        {
            CurrentHitPoint -= value;
        }

        /// <summary>
        /// Child will call this method when collision is detected.
        /// </summary>
        /// <param name="other">The collision parameter from <see cref="UnityEngine.OnTriggerEnter2D"/></param>
        public void TakeDamage(Collider2D other)
        {
        }

        public int GetCurrentDamage()
        {
            return Mathf.RoundToInt(Mathf.Lerp(Stat.DamageMin, Stat.DamageMax, _lastPerfectionPercent));
        }
        public void Die()
        {
            Destroy(Weapon.gameObject);
            Destroy(gameObject);
        }
    }
}
