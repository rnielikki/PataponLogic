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
        /// Animator (Can be replaced to Behaviour in future)
        /// </summary>
        protected PataponAnimator _animator { get; private set; }

        /// <summary>
        /// This object represents weapon to throw. Will be changed to <see cref="IEquipment.Object"/> later.
        /// </summary>
        protected GameObject ___temp;


        /// <summary>
        /// Remember call this on Awake() in inherited class
        /// </summary>
        protected void Init()
        {
            _animator = new PataponAnimator(GetComponent<Animator>());
            Stat = DefaultStat;
            Weapon = GetComponentInChildren<WeaponObject>();

            ___temp = transform.Find("Weapon").gameObject;
        }
        public void MoveOnDrum(string drumName) => _animator.Animate(drumName);
        /// <summary>
        /// Recieves command song and starts corresponding moving.
        /// </summary>
        /// <param name="song">The command song, which determines what the patapon will act.</param>
        public virtual void Act(CommandSong song, bool isFever) //maybe will moved to something like PataponAction. Let me see.
        {
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
        /// Going back to Idle status, also removes PONCHAKA <see cref="_charged"/> status.
        /// </summary>
        public void PlayIdle()
        {
            _charged = false;
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
            _animator.SetAttackSpeed(2 / Stat.AttackSeconds);
            StartCoroutine(_animator.AnimateInTime("attack", Stat.AttackSeconds));
        }
        /// <summary>
        /// CHAKACHAKA Input
        /// </summary>
        protected virtual void Defend(bool isFever)
        {
            _animator.SetAttackSpeed(2 / Stat.AttackSeconds);
            StartCoroutine(_animator.AnimateInTime("defend", Stat.AttackSeconds));
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
    }
}
