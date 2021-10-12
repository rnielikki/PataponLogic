using Core.Rhythm.Command;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Character.Patapon.Animation
{
    public class PataponAnimator
    {
        private readonly Animator _animator;
        private readonly Dictionary<CommandSong, string> _songAnimationMap =
            new Dictionary<CommandSong, string>()
            {
                { CommandSong.Patapata, "walk" },
                { CommandSong.Ponpon, "attack" },
                { CommandSong.Chakachaka, "defend" },
                { CommandSong.Ponpata, "dodge" },
                { CommandSong.Ponchaka, "charge" },
                { CommandSong.Dondon, "jump" },
                { CommandSong.Donchaka, "party" },
                { CommandSong.Patachaka, "Idle" },
                { CommandSong.None, "Idle" }
            };

        internal PataponAnimator(Animator animator)
        {
            _animator = animator;
        }
        public void Animate(string animationType)
        {
            _animator.Play(animationType, -1, 0f);
        }
        //We'll make this for each patapon units! :) Maybe something like GetAnimationBehaviour to patapon class?
        public void AnimateCommand(RhythmCommandModel command)
        {
            //Add PONCHAKA~PONPON STATUS!
            if (command.ComboType == ComboStatus.Fever && command.Song == CommandSong.Ponpon)
            {
                _animator.Play("attack-fever");
            }
            else
            {
                _animator.Play(_songAnimationMap[command.Song]);
            }
        }
    }
}
