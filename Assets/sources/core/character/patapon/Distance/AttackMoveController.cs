using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Ensures that Patapon attacks in right distance.
/// </summary>
namespace Core.Character.Patapon
{
    public class AttackMoveController : MonoBehaviour
    {
        private float _movingSpeed;
        private float _attackSeconds;
        private Animation.PataponAnimator _animator;
        private PataponDistance _pataponDistance;

        private AttackMoveModel _currentModel;
        private readonly Dictionary<string, AttackMoveModel> _attackMoves = new Dictionary<string, AttackMoveModel>();

        private Vector2 direction = Vector2.right; //can be changed later for enemy direction.
        private bool _moving;

        private byte _currentStatusFlag; //0: not assigned, 1: no target, 2: moving to target, 3: attacking

        //public float TotalOffset => _attackDistance + _sizeOffset;

        private bool _attacking;

        void Awake()
        {
            var patapon = GetComponent<Patapon>();
            _movingSpeed = patapon.Stat.MovementSpeed;
            _attackSeconds = patapon.Stat.AttackSeconds;
            _animator = patapon.PataponAnimator;
            _pataponDistance = GetComponent<PataponDistance>();
        }
        /// <summary>
        /// Adds model instructions for any type of future attacking command.
        /// </summary>
        /// <param name="attackMoves">Dictionary of the models for indexing.</param>
        /// <note>Register key is expected as animation for most cases, for understanding code. (exception : same animation for other type of attack)</note>
        internal void AddModels(Dictionary<string, AttackMoveModel> attackMoves)
        {
            foreach (var attackMove in attackMoves)
            {
                _attackMoves.Add(attackMove.Key, attackMove.Value);
            }
        }
        /// <summary>
        /// Starts attack. This also manages Patapon moving to distance before attacking.
        /// </summary>
        /// <param name="attackType">Attack type that was registered.</param>
        /// <note>If you followed <see cref="AddModels"/> instruction, then the attackType is animation name.</note>
        public void StartAttack(string attackType)
        {
            if (!_attackMoves.TryGetValue(attackType, out _currentModel))
            {
                throw new System.ArgumentException($"The attack animation type {attackType} is not registered.");
            }
            if (_currentModel.AlwaysAnimate)
            {
                _animator.Animate(_currentModel.AnimationType);
            }
            else
            {
                _animator.Animate("Idle");
            }
        }
        public void StopAttack()
        {
            StopAllCoroutines();
            _attacking = false;
            _moving = false;
            _currentModel = null;
            _currentStatusFlag = 0;
        }
        public bool IsInAttackDistance()
        {
            if (_currentModel == null) return false;
            return _pataponDistance.HasAttackTarget() && _pataponDistance.IsInTargetRange(_currentModel.GetDistance(), _movingSpeed * Time.deltaTime);
        }

        private void AnimateAttack()
        {
            StartCoroutine(DoAnimatingCoroutine());
            System.Collections.IEnumerator DoAnimatingCoroutine()
            {
                yield return _animator.AnimateAttack(_currentModel.AnimationType, _attackSeconds, _currentModel.AttackSpeedMultiplier);
            }
        }
        private void Update()
        {
            if (_currentModel == null)
            {
                if (_attacking) StopAttack();
                return;
            }
            else if (_currentModel.AlwaysAnimate)
            {
                transform.position = Vector2.MoveTowards(transform.position, _currentModel.GetDistance() * direction, _currentModel.MovingSpeed * Time.deltaTime);
                return;
            }
            var isAttackMove = _currentModel.Type == AttackMoveType.Attack;
            bool isInDistance = _currentModel.IsInAttackDistance();
            bool hasTarget = _currentModel.HasTarget();

            if (!hasTarget && isAttackMove) //1. No target in sight!
            {
                if (_currentStatusFlag != 1)
                {
                    _currentStatusFlag = 1;
                    if (_moving)
                    {
                        if (_pataponDistance.IsInTargetRange(_pataponDistance.DefaultWorldPosition, _movingSpeed * Time.deltaTime))
                        {
                            _animator.Animate("Idle");
                            _moving = false;
                        }
                    }
                    else
                    {
                        _moving = true;
                        _attacking = false;
                        StopAllCoroutines();
                        if (!_currentModel.AlwaysAnimate)
                        {
                            _animator.Animate("walk");
                        }
                    }
                }
                if (_moving)
                {
                    transform.position = Vector2.MoveTowards(transform.position, _pataponDistance.DefaultWorldPosition * Vector2.right, _movingSpeed * Time.deltaTime);
                }
            }
            else if (!isInDistance) //2. Found target.
            {
                if (_currentStatusFlag != 2)
                {
                    _currentStatusFlag = 2;
                    _moving = true;
                    _animator.Animate("walk");
                }
                transform.position = Vector2.MoveTowards(transform.position, _currentModel.GetDistance() * direction, _movingSpeed * Time.deltaTime);
            }
            else if (!_attacking && _currentStatusFlag != 3) //3. Finally, attacking.
            {
                _currentStatusFlag = 3;
                if (!_currentModel.AlwaysAnimate)
                {
                    AnimateAttack();
                }
                _attacking = true;
                _moving = false;
            }
        }
    }
}
