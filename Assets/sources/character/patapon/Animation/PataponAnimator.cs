using UnityEngine;
using Core.Rhythm;
using Core.Rhythm.Command;
using System.Collections.Generic;

namespace Core.Character.Patapon.Animation
{
    public class PataponAnimator : MonoBehaviour
    {
        private PataponAnimation[] _pataponAnimations;

        void Awake()
        {
            _pataponAnimations = GetComponentsInChildren<PataponAnimation>();
        }
        public void PlayDrumAnimations(RhythmInputModel model)
        {
            PlayAnimations(model.Drum.ToString());
        }
        public void PlayCommandAnimations(RhythmCommandModel commandModel)
        {
            foreach (var anim in _pataponAnimations)
            {
                anim.AnimateCommand(commandModel);
            }
        }
        public void PlayIdleAnimation() => PlayAnimations("Idle");
        private void PlayAnimations(string animationName)
        {
            foreach (var anim in _pataponAnimations) anim.Animate(animationName);
        }
    }
}
