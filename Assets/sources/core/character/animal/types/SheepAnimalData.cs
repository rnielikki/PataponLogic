using UnityEngine;

namespace PataRoad.Core.Character.Animal
{
    class SheepAnimalData : AttackingAnimalData
    {
        bool _isBeforeAttack;
        Vector3 _firstStepPoint;
        private void Start()
        {
            var point = Map.MissionPoint.Current.transform.position;
            point.x += 10;
            _firstStepPoint = point;
        }
        public override void OnTarget()
        {
            _isBeforeAttack = true;
            SetTarget(_firstStepPoint);
        }
        protected override void Update()
        {
            base.Update();
            if (_isBeforeAttack)
            {
                if (Move(true))
                {
                    _isBeforeAttack = false;
                    base.OnTarget();
                }
            }
            else
            {
                base.Update();
            }
        }
    }
}
