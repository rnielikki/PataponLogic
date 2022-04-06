using PataRoad.Core.Character.Equipments.Weapons;
using UnityEngine;

namespace PataRoad.Core.Character.Animal
{
    [DisallowMultipleComponent]
    class DefaultAnimalData : MonoBehaviour, IAnimalData
    {
        public AttackType AttackType { get; protected set; } = AttackType.Stab;

        [SerializeField]
        protected float _sight;
        public virtual float Sight => _sight;
        [SerializeField]
        private float _runningDistance;

        protected Vector3 _targetPosition;
        protected bool _moving;

        [SerializeField]
        private Stat _stat = Stat.GetAnyDefaultStatForCharacter();
        public Stat Stat => _stat;
        protected CharacterAnimator _animator;
        protected StatusEffectManager _statusEffectManager;
        protected AnimalBehaviour _behaviour;
        protected DistanceCalculator _distanceCalculator;

        [SerializeField]
        protected AudioClip _soundOnFound;

        public bool PerformingAction { get; protected set; }
        protected bool _useFixedTargetPosition;

        public virtual void InitFromParent(AnimalBehaviour parent)
        {
            _behaviour = parent;
            _animator = parent.CharAnimator;
            _statusEffectManager = parent.StatusEffectManager;
            _distanceCalculator = parent.DistanceCalculator;
            _statusEffectManager.AddRecoverAction(DoRecoverAction);
        }
        public virtual void OnTarget()
        {
            SetTarget((_behaviour.DefaultWorldPosition + _runningDistance) * Vector3.right);
        }
        public void StartMoving()
        {
            _moving = true;
        }
        public void OnDamaged()
        {
            if (CanMove()) OnTarget();
        }
        public virtual void StopAttacking()
        {
        }
        public virtual bool CanMove()
            => !PerformingAction && !_behaviour.IsDead
                && !_behaviour.StatusEffectManager.IsOnStatusEffect && !_useFixedTargetPosition;
        /// <summary>
        /// Moves animal to <see cref="_targetPosition"/>.
        /// </summary>
        /// <param name="endActionWhenMoved"></param>
        /// <returns><c>true</c> if animal position is same as target position, otherwise <c>false</c></returns>
        protected bool Move(bool endActionWhenMoved = true)
        {
            if (_moving)
            {
                var offset = Stat.MovementSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, offset);
                if (transform.position.x == _targetPosition.x)
                {
                    SetToIdle(endActionWhenMoved);
                    _statusEffectManager.IgnoreStatusEffect = false;
                    return true;
                }
                else if (!_useFixedTargetPosition && transform.position.x > Map.MissionPoint.Current.MissionPointPosition.x)
                {
                    _targetPosition.x = Map.MissionPoint.Current.MissionPointPosition.x + 99;
                    _useFixedTargetPosition = true;
                }
                return false;
            }
            return true;
        }
        protected void SetTarget(Vector3 position)
        {
            if (!CanMove()) return;
            PerformingAction = true;
            GameSound.SpeakManager.Current.Play(_soundOnFound);
            _statusEffectManager.IgnoreStatusEffect = true;
            _targetPosition = position;

            _animator.SetMoving(true);
        }
        protected virtual void DoRecoverAction(StatusEffectType type) => SetToIdle(true);
        private void SetToIdle(bool endActionWhenMoved)
        {
            _moving = false;
            if (endActionWhenMoved)
            {
                PerformingAction = false;
                _behaviour.SetCurrentAsWorldPosition();
            }
            _animator.SetMoving(false);
        }
        private void Update()
        {
            Move();
        }
    }
}
