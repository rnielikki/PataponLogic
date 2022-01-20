using UnityEngine;

namespace PataRoad.Core.Character
{
    /// <summary>
    /// Includes odd attacking black magic logic... well, maybe not really.
    /// </summary>
    public class CharacterAnimator
    {
        private readonly Animator _animator;
        public Animator Animator => _animator;

        //Late animation, only for >2 seconds attack speed.
        private bool _lateAnimating;
        private float _idleTime;
        private readonly ICharacter _target;
        private bool _isIdleOnLateAttack;
        private float _currentOffset;

        internal CharacterAnimator(Animator animator, ICharacter target)
        {
            _animator = animator;
            _target = target;
        }
        /// <summary>
        /// Sets attack speed. Note that 1 is normal scale here.
        /// </summary>
        /// <param name="speed">Default is 1. The smaller the value is, the faster the attack animation is.</param>
        internal void SetAttackSpeed(float speed)
        {
            _animator.SetFloat("AttackSpeedMultiplier", speed);
        }
        /// <summary>
        /// For huntable animal. Sets if the animal is moving away, when found the target.
        /// </summary>
        public void SetMoving(bool isMoving)
        {
            _animator.SetBool("moving", isMoving);
        }
        /// <summary>
        /// Performs animation, also ensure playing from zero offset without transition.
        /// </summary>
        /// <param name="animationType">The animation name from animator.</param>
        public void Animate(string animationType)
        {
            if (!_target.IsDead && _target.StatusEffectManager.CanContinue)
            {
                _animator.Play(animationType, -1, 0f);
            }
        }
        /// <summary>
        /// Play dying animation, with no dependency of status effect.
        /// </summary>
        public void PlayDyingAnimation()
        {
            Resume();
            _animator.Play("die", -1, 0f);
        }
        /// <summary>
        /// Freezes the current animation.
        /// </summary>
        public void Stop()
        {
            _animator.Play("Idle", -1, 0f);
            _animator.speed = 0;
        }
        public void Resume() => _animator.speed = 1;
        /// <summary>
        /// Set late animation status to zero.
        /// </summary>
        public void ClearLateAnimation()
        {
            if (_lateAnimating)
            {
                _lateAnimating = false;
                _currentOffset = 0;
            }
        }
        /// <summary>
        /// Animate with smooth transition.
        /// </summary>
        /// <param name="animationType">Type of animaion.</param>
        public void AnimateFrom(string animationType)
        {
            if (_target.StatusEffectManager.CanContinue)
            {
                _animator.CrossFade(animationType, 0.5f);
            }
        }
        /// <summary>
        /// Perform attack animation, in coroutine. Use <see cref="Animate"/> instead for constant gesture attack (like tatepon ponchaka~ponpon).
        /// </summary>
        /// <param name="animationType">Animation name in animator.</param>
        /// <param name="attackSecondsStat">Attack seconds in stat.</param>
        /// <param name="speed">Speed multiplier. For example, Yumipon fever attack is 3 times faster than normal, so it can be 3.</param>
        /// <returns>Yield seconds wating, for coroutine.</returns>
        internal System.Collections.IEnumerator AnimateAttack(string animationType, float attackSecondsStat, float speed = 1)
        {
            var seconds = attackSecondsStat / speed;

            if (seconds <= Rhythm.RhythmEnvironment.TurnSeconds)
            {
                SetAttackSpeed(Rhythm.RhythmEnvironment.TurnSeconds / seconds);
                for (float i = 0; i < Rhythm.RhythmEnvironment.TurnSeconds; i += seconds)
                {
                    Animate(animationType);
                    yield return new WaitForSeconds(seconds);
                }
            }
            else
            {
                yield return AnimateLate(seconds, animationType);
            }
        }

        internal System.Collections.IEnumerator AnimateLate(float attackSeconds, string animationType)
        {
            if (!_lateAnimating) InitLateAnimation(attackSeconds);
            float offset = _currentOffset;
            float idleAnimationLength = -1;

            while (true)
            {
                if (_isIdleOnLateAttack)
                {
                    if (idleAnimationLength < 0)
                    {
                        _animator.Play("Idle", -1, offset % 1);
                        idleAnimationLength = _animator.GetCurrentAnimatorClipInfo(0).Length;
                    }
                    float idleLength = idleAnimationLength;
                    if (offset != 0)
                    {
                        idleLength -= offset * idleAnimationLength;
                        offset = 0;
                    }
                    yield return new WaitForSeconds(idleLength);
                    _currentOffset += idleLength;

                    if (_idleTime <= _currentOffset + 0.00001f) //just aware of float calculation
                    {
                        _isIdleOnLateAttack = false;
                        _currentOffset = 0;
                        idleAnimationLength = -1;
                    }
                }
                else
                {
                    _animator.Play(animationType, -1, offset);
                    yield return new WaitForSeconds(Rhythm.RhythmEnvironment.TurnSeconds);
                    _isIdleOnLateAttack = true;
                }
            }
        }
        public void PauseAttack()
        {
            if (_lateAnimating) _currentOffset += _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        private void InitLateAnimation(float attackSeconds)
        {
            SetAttackSpeed(1);
            _currentOffset = 0;
            _lateAnimating = true;
            _isIdleOnLateAttack = true;
            _idleTime = attackSeconds - Rhythm.RhythmEnvironment.TurnSeconds;
        }
    }
}
