using Core.Character.Equipment;
using Core.Character.Patapon.Animation;
using Core.Rhythm.Command;
using System;
using UnityEngine;

namespace Core.Character.Patapon
{
    public abstract class Patapon : MonoBehaviour, ICharacter
    {
        public IEquipment Helm { get; protected set; }
        public IEquipment Weapon { get; protected set; }
        public IEquipment Protector { get; protected set; }
        public Stat Stat { get; protected set; }

        public ClassType Class { get; protected set; }
        public Rarepon Rarepon { get; protected set; }

        protected bool _charged { get; private set; }

        protected PataponAnimator _animator { get; private set; }
        /// <summary>
        /// Remember call this on Awake() in inherited class
        /// </summary>
        protected void Init()
        {
            _animator = new PataponAnimator(GetComponent<Animator>());
            Rarepon = new Rarepon();
            Stat = Rarepon.Stat;
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
            _charged = song == CommandSong.Ponchaka;
        }

        public void PlayIdle() => _animator.Animate("Idle");

        void Walk()
        {
            _animator.Animate("walk");
        }
        protected virtual void Attack(bool isFever)
        {
            _animator.Animate("attack");
        }
        protected virtual void Defend(bool isFever)
        {
            _animator.Animate("defend");
        }
        void Dodge()
        {
            _animator.Animate("dodge");
        }
        protected virtual void Charge()
        {
            _animator.Animate("charge");
        }
        void Jump()
        {
            _animator.Animate("jump");
        }
        void Party()
        {
            _animator.Animate("party");
        }
    }
}
