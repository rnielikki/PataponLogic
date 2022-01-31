using UnityEngine;

namespace PataRoad.Core.Character.Animal
{
    class AttackingAnimalData : DefaultAnimalData
    {
        [SerializeField]
        [Tooltip("How many times it will attack while on attack mode. After certain times it'll move away")]
        int _attackCount;
        private int _currentAttackCount;
        [SerializeField]
        float _attackDistance;
        [SerializeField]
        private bool _flipWhenAttacking;

        private bool _willAttack;

        [SerializeField]
        protected UnityEngine.Events.UnityEvent _onAttack;
        [SerializeField]
        protected UnityEngine.Events.UnityEvent _onStopAttacking;

        private void Start()
        {
            _attackDistance += (GetComponentInChildren<Collider2D>().bounds.size.x / 2);
        }
        public void ChangeAttackType(Equipments.Weapons.AttackType attackType) => AttackType = attackType;
        public override void OnTarget()
        {
            if (!CanMove()) return;
            PerformingAction = true;
            _statusEffectManager.IgnoreStatusEffect = true;
            SetAttackPosition();
            _willAttack = true;
            if (_flipWhenAttacking) Flip(true);
            _animator.SetMoving(true);
        }
        public void StartAttack() => _onAttack.Invoke();
        public void EndAttack()
        {
            StopAttacking();
            _currentAttackCount--;
            if (_currentAttackCount <= 0)
            {
                if (_flipWhenAttacking) Flip(false);
                _behaviour.SetCurrentAsWorldPosition();
                PerformingAction = false;
                base.OnTarget();
                StartMoving();
                _animator.Animate("move");
            }
        }
        private void SetAttackPosition()
        {
            var closest = _distanceCalculator.GetTargetOnSight(CharacterEnvironment.Sight);
            if (closest == null)
            {
                _willAttack = false;
                _moving = false;
                _animator.Animate("Idle");
                return;
            }
            var pos = transform.position;
            pos.x = closest.Value.x + _attackDistance;
            _targetPosition = pos;
        }
        public override void StopAttacking() => _onStopAttacking.Invoke();
        private bool CanGoForward()
        {
            var closest = _behaviour.DistanceCalculator.GetClosest();
            return closest == null || closest.Value.x < transform.position.x - _behaviour.CharacterSize;
        }
        private void Flip(bool towardsPatapons)
        {
            var sc = transform.localScale;
            sc.x = towardsPatapons ? -1 : 1;
            transform.localScale = sc;
        }
        private void Update()
        {
            if (_moving)
            {
                //moving for attacking.
                if (_willAttack)
                {
                    if (Move(false) || !CanGoForward())
                    {
                        _moving = false;
                        _willAttack = false;
                        _animator.SetMoving(false);
                        _currentAttackCount = _attackCount;
                        _animator.Animate("attack");
                    }
                }
                else
                {
                    //moving for moving.
                    Move(true);
                }
            }
        }
    }
}
