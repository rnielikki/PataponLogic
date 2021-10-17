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
        public int CurrentHitPoint { get; protected set; }
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
        /// Remember call this on Awake() in inherited class
        /// </summary>
        protected void Init()
        {
            _animator = new PataponAnimator(GetComponent<Animator>());
            Stat = DefaultStat;
            Weapon = GetComponentInChildren<WeaponObject>();
        }
        public void MoveOnDrum(string drumName) => _animator.Animate(drumName);
        /// <summary>
        /// Recieves command song and starts corresponding moving.
        /// </summary>
        /// <param name="song">The command song, which determines what the patapon will act.</param>
        public virtual void Act(CommandSong song, bool isFever)
        {
            StopAllCoroutines();
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
        }
        /// <summary>
        /// PATAPATA Input
        /// </summary>
        void Walk()
        {
            _animator.Animate("walk");
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
            AttackInTime("defend");
        }
        /// <summary>
        /// PONPATA Input
        /// </summary>
        protected virtual void Dodge()
        {
            _animator.Animate("dodge");
        }
        /// <summary>
        /// PONCHAKA Input
        /// </summary>
        protected virtual void Charge()
        {
            _animator.Animate("charge");
        }

        /// <summary>
        /// DONDON Input
        /// </summary>
        protected virtual void Jump()
        {
            _animator.Animate("jump");
        }

        /// <summary>
        /// DONCHAKA Input
        /// </summary>
        protected virtual void Party()
        {
            _animator.Animate("party");
        }
        public void WeaponAttack(AttackType type) => Weapon.Attack(type);

        /// <summary>
        /// Attack in time
        /// </summary>
        /// <param name="animationType">animation name in animator.</param>
        /// <param name="attackOffset">When appears attack status in the animation. This determines e.g. throwing. For example, if it's 0.5f, the attack starts in half of animation.</param>
        /// <param name="attackType">Attack Type, which can determine what kind of attack can do, depends on the weapon.</param>
        /// <param name="speed">Speed multiplier. For example, Yumipon fever attack is 3 times faster than normal, so it can be 3.</param>
        protected void AttackInTime(string animationType, float speed = 1)
        {
            StartCoroutine(AttackCoroutine());
            System.Collections.IEnumerator AttackCoroutine()
            {
                var seconds = Stat.AttackSeconds / speed;
                _animator.SetAttackSpeed(2 / seconds);

                for (float i = 0; i < Rhythm.RhythmEnvironment.TurnSeconds; i += seconds)
                {
                    _animator.Animate(animationType);
                    yield return new WaitForSeconds(seconds);
                }
            }
        }
    }
}
