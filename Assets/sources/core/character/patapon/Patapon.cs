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
        protected PataponAnimator _animator { get; private set; }

        /// <summary>
        /// Sets Patapon distance.
        /// </summary>
        protected PataponDistance _pataponDistance { get; private set; }

        /// <summary>
        /// Current Patapon Index, from first of the line to the end of the line. Index starts from 0.
        /// </summary>
        public int Index { get; internal set; }
        /// <summary>
        /// Current Patapon Index IN <see cref="PataponGroup"/>, from first of the line to the end of the line. Index starts from 0.
        /// </summary>
        public int IndexInGroup { get; internal set; }

        private CommandSong _lastSong;

        /// <summary>
        /// Attack distance, INCLUDING the Patapon size (Patapon radius in most case, except vehicle).
        /// </summary>
        public float AttackDistanceWithOffset => _pataponDistance.AttackDistanceWithOffset;

        /// <summary>
        /// Remember call this on Awake() in inherited class
        /// </summary>
        protected void Init()
        {
            _pataponDistance = GetComponent<PataponDistance>();
            _animator = new PataponAnimator(GetComponent<Animator>());
            Stat = DefaultStat;
            Weapon = GetComponentInChildren<WeaponObject>();
        }
        /// <summary>
        /// Sets distance from calculated Patapon head. Don't use this if the Patapon uses any vehicle.
        /// </summary>
        /// <param name="attackDistance">Attack distance, without considering head size.</param>
        protected void InitDistanceFromHead(float attackDistance)
        {
            _pataponDistance.InitDistance(
                attackDistance,
                transform.Find("Patapon-body/Face").GetComponent<CircleCollider2D>().radius + 0.1f
            );
        }

        public void MoveOnDrum(string drumName)
        {
            StopAllCoroutines();
            _animator.Animate(drumName);
            _pataponDistance.StopMoving();
        }

        /// <summary>
        /// Recieves command song and starts corresponding moving.
        /// </summary>
        /// <param name="song">The command song, which determines what the patapon will act.</param>
        public virtual void Act(CommandSong song, bool isFever)
        {
            StopAllCoroutines();
            if (_lastSong != song) _animator.ClearLateAnimation();
            _lastSong = song;
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
                    _animator.Animate("walk");
                    _pataponDistance.MoveToInitialPlace(Stat.MovementSpeed * 2);
                    break;
            }
            _charged = song == CommandSong.Ponchaka; //Removes charged status if it isn't charging command
        }

        /// <summary>
        /// Going back to Idle status. This also means ALL FEVER/COMMAND is CANCELED. also removes PONCHAKA <see cref="_charged"/> status.
        /// </summary>
        public virtual void PlayIdle()
        {
            _charged = false;
            StopAllCoroutines();
            _animator.Animate("Idle");
            _pataponDistance.MoveToInitialPlace(Stat.MovementSpeed);
        }
        /// <summary>
        /// PATAPATA Input
        /// </summary>
        void Walk()
        {
            _animator.Animate("walk");
            _pataponDistance.MoveToInitialPlace(Stat.MovementSpeed);
        }
        /// <summary>
        /// PONPON Input
        /// </summary>
        protected virtual void Attack(bool isFever)
        {
            AttackInTime("attack");
        }
        /// <summary>
        /// CHAKACHAKA Input
        /// </summary>
        protected virtual void Defend(bool isFever)
        {
            AttackInTime("defend", defend: true);
        }
        /// <summary>
        /// PONPATA Input
        /// </summary>
        protected virtual void Dodge()
        {
            _animator.Animate("dodge");
            _pataponDistance.MoveBack(Stat.MovementSpeed * 1.5f);
        }
        /// <summary>
        /// PONCHAKA Input
        /// </summary>
        protected virtual void Charge()
        {
            _animator.Animate("charge");
            _pataponDistance.MoveToInitialPlace(Stat.MovementSpeed);
        }

        /// <summary>
        /// DONDON Input
        /// </summary>
        protected virtual void Jump()
        {
            _animator.Animate("jump");
            _pataponDistance.StopMoving();
        }

        /// <summary>
        /// DONCHAKA Input
        /// </summary>
        protected virtual void Party()
        {
            _animator.Animate("party");
            _pataponDistance.MoveToInitialPlace(Stat.MovementSpeed);
        }
        public void WeaponAttack(AttackCommandType type) => Weapon.Attack(type);

        /// <summary>
        /// Performs attack animation, applying attack seconds in stat.
        /// </summary>
        /// <param name="animationType">Animation name in animator.</param>
        /// <param name="speed">Speed multiplier. For example, Yumipon fever attack is 3 times faster than normal, so it can be 3.</param>
        protected void AttackInTime(string animationType, float speed = 1, bool defend = false)
        {
            if (!_pataponDistance.HasAttackTarget()) return;
            StartCoroutine(WalkAndAttack());
            System.Collections.IEnumerator WalkAndAttack()
            {
                _animator.Animate("walk");
                if (defend)
                {
                    yield return _pataponDistance.MoveToDefend(Stat.MovementSpeed);
                }
                else
                {
                    yield return _pataponDistance.MoveToAttack(Stat.MovementSpeed);
                }
                yield return _animator.AnimateAttack(animationType, Stat.AttackSeconds, speed);
            }
        }

        public void TakeDamage(int value)
        {
            CurrentHitPoint -= value;
        }

        /// <summary>
        /// Child will call this method when collision is detected.
        /// </summary>
        /// <param name="other">The collision parameter from <see cref="UnityEngine.OnCollisionEnter2D"/></param>
        public void TakeCollision(Collision2D other)
        {
        }
    }
}
