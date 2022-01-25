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

        public void Animate(string animation)
        {
            _animator.Play(animation);
            StopAllCoroutines();
            _inturrupted = true;
            _walking = false;
        }
        public void AnimateOnce(string animation)
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

        public void SetToDefaultWalkingStep() => _walkingStep = _defaultWalkingStep;
        public void SetWalkingStep(float steps) => _walkingStep = steps;

        public void Walk(float steps) => WalkTowards(transform.position.x + steps * transform.localScale.x);
        public void Move(float steps) => MoveTowards(transform.position.x + steps * transform.localScale.x);

        public void WalkTowards(float position)
        {
            MoveTowards(position);
            _animator.SetBool("walking", true);
            _animatingWalking = true;
        }
        public void MoveTowards(float position)
        {
            StopAllCoroutines();
            _inturrupted = true;
            _walking = true;
            _targetPosition = position * Vector2.right;
        }

        public void Flip()
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

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
