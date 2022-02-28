using UnityEngine;

namespace PataRoad.Core.Character.Animal
{
    class SheepAnimalData : AttackingAnimalData
    {
        bool _isBeforeAttack;
        Vector3 _firstStepPoint;
        private float _defaultSight;
        private float _currentSight;
        public override float Sight => _currentSight;
        private bool _idle;
        private void Start()
        {
            var point = Map.MissionPoint.Current.MissionPointPosition;
            point.x--;
            _firstStepPoint = point;
            _idle = true;
            _currentSight = _defaultSight = _sight;
        }
        public override void OnTarget()
        {
            if (!CanMove() || !_idle) return;
            if (_isBeforeAttack) // 3
            {
                base.OnTarget();
                _isBeforeAttack = false;
                _currentSight = _defaultSight;
            }
            else // 1
            {
                SetTarget(_firstStepPoint);
                _isBeforeAttack = true;
                _willAttack = false;
            }
            _idle = false;
        }
        public void EnterIdle() => _idle = true;
        protected override void Update()
        {
            if (!_moving) return;
            if (_isBeforeAttack)
            {
                if (Move(true)) // 2
                {
                    _currentSight = CharacterEnvironment.Sight;
                    _idle = false;
                    _moving = false;
                    _animator.SetMoving(false);
                }
            }
            else
            {
                base.Update();
            }
        }
    }
}
