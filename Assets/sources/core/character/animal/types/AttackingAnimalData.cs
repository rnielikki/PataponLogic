using UnityEngine;

namespace PataRoad.Core.Character.Animal
{
    class AttackingAnimalData : DefaultAnimalData
    {
        [SerializeField]
        [Tooltip("How many times it will attack while on attack mode. After certain times it'll move away")]
        int _attackCount;
        [SerializeField]
        float _attackDistance;
        [SerializeField]
        bool _flipOnAttack;

        private bool _willAttack;

        private void Start()
        {
            _attackDistance += (GetComponentInChildren<Collider2D>().bounds.size.x / 2);
        }
        public override void OnTarget()
        {
            PerformingAction = true;
            _statusEffectManager.IgnoreStatusEffect = true;
            //Flip();
            _willAttack = true;
            _animator.SetMoving(true);
        }
        public void Attack()
        {
            //damage?
        }
        public void EndAttack()
        {
            //damage?
            _animator.Animator.SetBool("attacking", false);
            //Flip();
            base.OnTarget();
        }
        private void Flip()
        {
            if (!_flipOnAttack) return;
            var scale = transform.localScale;
            scale.x *= -1;
            transform.position = scale;
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
        private void Update()
        {
            if (_moving)
            {
                if (_willAttack)
                {
                    SetAttackPosition();
                    if (Move(true) || _targetPosition.x > transform.position.x)
                    {
                        _moving = false;
                        _willAttack = false;
                        _animator.Animator.SetBool("attacking", true);
                    }
                }
                else
                {
                    Move();
                }
            }
        }
    }
}
