using System;
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
        [Tooltip("The unique value. also can be used to set character order.")]
        private int _uniqueSeed;
        internal int Seed => _uniqueSeed;

        private Animator _animator;
        private StoryBehaviour _instance;

#pragma warning disable S1450 // Shut up, TWO METHODS are using it
        private bool _inturrupted;
#pragma warning restore S1450 // Private fields only used as local variables in methods should become local variables

        private bool _walking;
        private bool _animatingWalking;
        private Vector2 _targetPosition;

        private float _walkingStep;
        private const float _defaultWalkingStep = 8;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _walkingStep = _defaultWalkingStep;

            foreach (var img in GetComponentsInChildren<SpriteRenderer>(true))
            {
                img.sortingOrder = _uniqueSeed;
            }
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

        public void Walk(float steps) => _instance.WalkTowardsInReal(transform.position.x + steps * transform.localScale.x);
        public void WalkTowards(float position) => _instance.WalkTowardsInReal(position);

        public void Move(float steps) => _instance.MoveTowardsInReal(transform.position.x + steps * transform.localScale.x);
        public void MoveTowards(float steps) => _instance.MoveTowardsInReal(steps);

        private void WalkTowardsInReal(float position)
        {
            MoveTowardsInReal(position);
            _animator.SetBool("walking", true);
            _animatingWalking = true;
        }
        private void MoveTowardsInReal(float position)
        {
            StopAllCoroutines();
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
        public void Show() => _instance.gameObject.SetActive(true);
        public void Hide() => _instance.gameObject.SetActive(false);

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
                    if (_animatingWalking)
                    {
                        _animatingWalking = false;
                        _animator.SetBool("walking", false);
                    }
                }
            }
        }
        private void OnValidate()
        {
            if (tag == "SmallCharacter" && !GetComponent<Animator>().runtimeAnimatorController.name.StartsWith("Story"))
            {
                Debug.LogError($"Please use story animator for instead of [{GetComponent<Animator>().runtimeAnimatorController.name}] the character, or remove the script if it's not for not story mode.");
            }
        }
    }
}
