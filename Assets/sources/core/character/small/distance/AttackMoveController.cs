using System.Collections.Generic;
using UnityEngine;
namespace PataRoad.Core.Character
{
    /// <summary>
    /// Ensures that Patapon attacks in right distance. Works with black magic.
    /// </summary>
    public class AttackMoveController : MonoBehaviour
    {
        /// <summary>
        /// <c>true</c> if it's hit in last time. if it doesn't, it may stop animating and move.
        /// </summary>
        private SmallCharacter _character;
        private float _movingSpeed => _character.Stat.MovementSpeed;
        private float _attackSeconds => _character.Stat.AttackSeconds;
        private CharacterAnimator _animator;

        private AttackMoveModel _currentModel;
        private readonly Dictionary<string, AttackMoveModel> _attackMoves = new Dictionary<string, AttackMoveModel>();

        private bool _moving;

        private byte _currentStatusFlag; //0: not assigned, 1: no target, 2: moving to target, 3: attacking

        private bool _attacking;
        private IAttackMoveData _data;

        private DistanceCalculator _distanceCalculator;
        private bool _movingRight;

        private bool _onAttackCommand;

        void Awake()
        {
            _character = GetComponent<SmallCharacter>();
            _data = _character.ClassData.AttackMoveData;
            _animator = _character.CharAnimator;
            _distanceCalculator = _character.DistanceCalculator;
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
            else if (_data.WasHitLastTime && _currentModel.HasTarget())
            {
                AnimateAttack();
            }
            else
            {
                _animator.Animate("Idle");
            }
            _onAttackCommand = true;
        }
        /// <summary>
        /// Stops attacking.
        /// </summary>
        /// <param name="pause">Will pause attack. If <c>true</c>, it'll continue attack after the turn.</param>
        public void StopAttack(bool pause) => StopAttack(pause, false);
        private void StopAttack(bool pause, bool keepAttackCommand)
        {
            if (pause) _character.CharAnimator.PauseAttack();
            else _character.CharAnimator.ClearLateAnimation();
            StopAllCoroutines();
            _attacking = false;

            _onAttackCommand = keepAttackCommand;

            if (!keepAttackCommand)
            {
                _moving = false;
                _currentModel = null;
                _currentStatusFlag = 0;
            }
        }

        private void AnimateAttack()
        {
            _currentStatusFlag = 3;
            if (_currentModel.AlwaysAnimate) return;
            StartCoroutine(DoAnimatingCoroutine());
            System.Collections.IEnumerator DoAnimatingCoroutine()
            {
                _moving = false;
                _attacking = true;
                _data.WasHitLastTime = true;
                do
                {
                    yield return _animator.AnimateAttack(_currentModel.AnimationType, _attackSeconds, _currentModel.AttackSpeedMultiplier);
                } while (_data.WasHitLastTime);
                _attacking = false;
                _currentStatusFlag = 0;
            }
        }
        private void Update()
        {
            if (!_character.StatusEffectManager.CanContinue || !_onAttackCommand) return;
            if (_currentModel == null)
            {
                if (_attacking) StopAttack(false);
                return;
            }
            else if (_currentModel.AlwaysAnimate)
            {
                transform.position = Vector2.MoveTowards(transform.position, _distanceCalculator.GetSafeForwardPosition(_currentModel.GetPosition()) * Vector2.right, _currentModel.MovingSpeed * Time.deltaTime);
                return;
            }
            else if (_attacking) return;

            bool isInDistance = _currentModel.IsInAttackDistance();
            bool hasTarget = _currentModel.HasTarget();

            if (!hasTarget && _currentModel.Type == AttackMoveType.Attack) //1. No target in sight!
            {
                if (_currentStatusFlag != 1)
                {
                    _currentStatusFlag = 1;
                    StopAttack(false, true);
                }
                var onDefaultPosition = _character.transform.position.x == _character.DefaultWorldPosition;
                if (!_moving && !onDefaultPosition)
                {
                    _animator.Animate("walk");
                    _moving = true;
                    MoveTowards(_character.DefaultWorldPosition);
                }
                else if (_moving && onDefaultPosition)
                {
                    _animator.Animate("Idle");
                    _moving = false;
                }
            }
            else if (!isInDistance) //2. Found target.
            {
                if (_currentStatusFlag != 2)
                {
                    _currentStatusFlag = 2;
                    _moving = true;
                    StopAttack(false, true);
                    _animator.Animate("walk");
                }
                MoveTowards(_distanceCalculator.GetSafeForwardPosition(_currentModel.GetPosition()));
            }
            else if (!_attacking && _currentStatusFlag != 3) //3. Finally, attacking.
            {
                AnimateAttack();
            }
        }
        //Don't waste time for moving character, just attack!
        private void MoveTowards(float pos)
        {
            var savedPosition = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, pos * Vector2.right, _movingSpeed * Time.deltaTime);
            var movingRight = transform.position.x < savedPosition.x;

            if (_movingRight != movingRight) AnimateAttack();
            _movingRight = movingRight;
        }
    }
}
