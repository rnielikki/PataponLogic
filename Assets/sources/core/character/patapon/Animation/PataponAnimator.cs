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
        public IEnumerator AnimateInTime(string animationType, float seconds)
        {
            for (int i = 0; i <= seconds + 0.01f; i++)
            {
                _animator.Play(animationType, -1, 0f);
                yield return new WaitForSeconds(seconds);
            }
        }
    }
}
