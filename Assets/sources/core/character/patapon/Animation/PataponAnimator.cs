using System.Collections;
using UnityEngine;

namespace Core.Character.Patapon.Animation
{
    public class PataponAnimator
    {
        private readonly Animator _animator;
        internal PataponAnimator(Animator animator)
        {
            _animator = animator;
        }
        /// <summary>
        /// Sets attack speed. Note that 1 is normal scale here.
        /// </summary>
        /// <param name="speed">Default is 1. The smaller the value is, the faster the attack animation is.</param>
        internal void SetAttackSpeed(float speed)
        {
            _animator.SetFloat("AttackSpeedMultiplier", speed);
        }
        public void Animate(string animationType)
        {
            _animator.Play(animationType, -1, 0f);
        }
        /// <summary>
        /// Animate with smooth transition.
        /// </summary>
        /// <param name="animationType">Type of animaion.</param>
        public void AnimateFrom(string animationType)
        {
            _animator.CrossFade(animationType, 0.5f);
        }
    }
}
