using PataRoad.Core.Character.Equipments.Weapons;
using UnityEngine;

namespace PataRoad.Core.Character
{
    class CannonStructure : Structure, IAttacker
    {
        private GameObject _weaponInstanceResource;
        protected bool _started;

        [SerializeField]
        private Transform _pipe;
        [SerializeField]
        private SpriteRenderer _bullet;
        [SerializeField]
        float _minAngle = 15;
        [SerializeField]
        float _maxAngle = 45;
        [SerializeField]
        float _rotateSpeed = 10;
        float _targetAngle;
        float _currentAngle = 90;
        private bool _changingAngle;
        private bool _attackAfterLooking = true;
        /// <summary>
        /// Use animator for updating angle.
        /// </summary>
        public bool IsAnimatorUpdatingAngle { get; set; }

        [SerializeField]
        float _minPower = 2;
        [SerializeField]
        float _maxPower = 2.4f;
        [SerializeField]
        float _bulletMass = 2;
        [SerializeField]
        bool _repeatAttack = true;
        [SerializeField]
        bool _autoAttack;

        [SerializeField]
        private AttackType _attackType;
        public AttackType AttackType => _attackType;

        [SerializeField]
        private ElementalAttackType _elementalAttackType;
        public ElementalAttackType ElementalAttackType => _elementalAttackType;

        [SerializeField]
        private int _damage;

        protected override void Start()
        {
            base.Start();
            _weaponInstanceResource = WeaponInstance.GetResource();
            Stat.DamageMax += _damage;
            if (_autoAttack) SetAttack();
        }
        private System.Collections.IEnumerator AnimateAttack()
        {
            yield return new WaitForSeconds(2);
            ReadyForAttack();
        }
        public void StartAttack()
        {
            IsAnimatorUpdatingAngle = false;
            ReadyForAttack();
        }
        private void ReadyForAttack()
        {
            _targetAngle = 90 - Random.Range(_minAngle, _maxAngle);
            _attackAfterLooking = true;
            _changingAngle = true;
        }
        protected virtual void Attack()
        {
            ThrowBullet();
            if (_repeatAttack) StartCoroutine(AnimateAttack());
            else
            {
                _targetAngle = 90;
                _attackAfterLooking = false;
                _changingAngle = true;
            }
        }
        protected void ThrowBullet()
        {
            var instantiated = Instantiate(_weaponInstanceResource);
            instantiated.GetComponent<WeaponInstance>()
                .Initialize(this, _bullet.sprite, _bullet.material, _bullet.gameObject.layer, _bulletMass, _bullet.transform)
                .Throw(_minPower, _maxPower);
        }
        public void OnAttackHit(Vector2 point, int damage)
        {
            //keep attacking
        }

        public void OnAttackMiss(Vector2 point)
        {
            //adjust angle
        }
        public float GetAttackValueOffset() => 1;
        public override void SetLevel(int level, int absoluteMaxLevel)
        {
            base.SetLevel(level, absoluteMaxLevel);
            Stat.DamageMax += level * 2;
        }
        public virtual void SetAttack()
        {
            _started = true;
            StartCoroutine(AnimateAttack());
        }
        public void StopAttacking()
        {
            IsAnimatorUpdatingAngle = true;
            StopAllCoroutines();
            _animator.Play("Idle");
        }
        public override void Die()
        {
            StopAttacking();
            base.Die();
        }
        private void LateUpdate()
        {
            if (IsDead || IsAnimatorUpdatingAngle) return;
            if (_changingAngle)
            {
                float angle = _currentAngle;
                if (_targetAngle > angle) angle = Mathf.Min(angle + (Time.deltaTime * _rotateSpeed), _targetAngle);
                else if (_targetAngle < angle) angle = Mathf.Max(angle - (Time.deltaTime * _rotateSpeed), _targetAngle);
                else
                {
                    _changingAngle = false;
                    if (_attackAfterLooking) _animator.Play("attack");
                }
                _currentAngle = angle;
            }
            _pipe.transform.eulerAngles = Vector3.forward * _currentAngle;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_started || !collision.CompareTag("Ground")) return;
            SetAttack();
        }
    }
}
