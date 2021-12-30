using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Story.Actions
{
    /// <summary>
    /// Adds behaviour to the story. Also used for mapping, see more on <see cref="StoryResourceMapper"/>.
    /// </summary>

    class StoryBehaviour : MonoBehaviour
    {
        [SerializeField]
        private int _uniqueSeed;
        internal int Seed => _uniqueSeed;

        private Animator _animator;
        private StoryBehaviour _instance;

#pragma warning disable S1450 // Shut up, TWO METHODS are using it
        private bool _inturrupted;
#pragma warning restore S1450 // Private fields only used as local variables in methods should become local variables

        private bool _walking;
        private Vector2 _targetPosition;

        private float _walkingStep;
        private string _walkingAnimation = "walk";
        private const float _defaultWalkingStep = 2;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _walkingStep = _defaultWalkingStep;
        }
        internal void SetInstance(StoryBehaviour instance) => _instance = instance;

        public void Animate(string animation) => _instance.AnimateInReal(animation);
        private void AnimateInReal(string animation)
        {
            _animator.Play(animation);
            StopAllCoroutines();
            _inturrupted = true;
            _walking = false;
        }
        public void AnimateOnce(string animation) => _instance.AnimateOnceInReal(animation);
        private void AnimateOnceInReal(string animation)
        {
            _animator.Play(animation);
            _walking = false;
            StartCoroutine(Wait());
            System.Collections.IEnumerator Wait()
            {
                yield return new WaitForEndOfFrame();
                var clip = _animator.GetCurrentAnimatorStateInfo(0);
                if (clip.IsName(animation)) //if no such animation may not being played
                {
                    _inturrupted = false;
                    yield return new WaitForSeconds(clip.length);
                    if (!_inturrupted) _animator.Play("Idle");
                }
            }
        }

        public void SetToDefaultWalkingStep() => _instance._walkingStep = _defaultWalkingStep;
        public void SetWalkingStep(float steps) => _instance._walkingStep = steps;
        public void SetWalkingAnimation(string animationName) => _instance._walkingAnimation = animationName;

        public void Walk(float steps) => _instance.WalkInReal(steps);
        private void WalkInReal(float steps) => WalkTowardsInReal(transform.position.x + steps * transform.localScale.x);
        public void WalkTowards(float position) => _instance.WalkTowardsInReal(position);
        private void WalkTowardsInReal(float position)
        {
            StopAllCoroutines();
            _animator.Play(_walkingAnimation);
            _inturrupted = true;
            _walking = true;
            _targetPosition = position * Vector2.right;
        }

        public void Flip() => _instance.FlipInReal();
        private void FlipInReal()
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        void WeaponAttack()
        {
            //oh no the animation event
        }
        void StopWeaponAttacking()
        {
            //oh no the animation event
        }
        private void Update()
        {
            if (_walking)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _walkingStep * Time.deltaTime);
                if (transform.position.x == _targetPosition.x)
                {
                    _walking = false;
                    _animator.Play("Idle");
                }
            }
        }
    }
}
