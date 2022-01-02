using PataRoad.Core.Character.Equipments.Weapons;
using UnityEngine;

namespace PataRoad.Core.Character.Animal
{
    class DefaultAnimalData : MonoBehaviour, IAnimalData
    {
        public AttackType AttackType => AttackType.Stab;

        [SerializeField]
        private float _sight;
        public float Sight => _sight;
        [SerializeField]
        private float _runningDistance;

        protected Vector3 _targetPosition;
        protected bool _moving;

        [SerializeField]
        private Stat _stat = Stat.GetAnyDefaultStatForCharacter();
        public Stat Stat => _stat;
        protected CharacterAnimator _animator;
        protected StatusEffectManager _statusEffectManager;
        protected DistanceCalculator _distanceCalculator;

        [SerializeField]
        protected AudioClip _soundOnFound;

        public bool PerformingAction { get; protected set; }

        public virtual void InitFromParent(AnimalBehaviour parent)
        {
            _animator = parent.CharAnimator;
            _statusEffectManager = parent.StatusEffectManager;
            _distanceCalculator = parent.DistanceCalculator;
        }
        public virtual void OnTarget()
        {
            PerformingAction = true;
            GameSound.SpeakManager.Current.Play(_soundOnFound);
            _statusEffectManager.IgnoreStatusEffect = true;
            _targetPosition = transform.position + _runningDistance * Vector3.right;

            _animator.SetMoving(true);
        }
        public void StartMoving()
        {
            _moving = true;
        }
        public void OnDamaged()
        {
            if (!PerformingAction) OnTarget();
        }
        public virtual void StopAttacking()
        {
        }
        protected bool Move(bool endActionWhenMoved = true)
        {
            if (_moving)
            {
                var offset = Stat.MovementSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, offset);
                if (transform.position.x == _targetPosition.x)
                {
                    _moving = false;
                    if (endActionWhenMoved) PerformingAction = false;
                    _animator.SetMoving(false);
                    _statusEffectManager.IgnoreStatusEffect = false;
                    return true;
                }
            }
            return false;
        }
        private void Update()
        {
            Move();
        }
    }
}
