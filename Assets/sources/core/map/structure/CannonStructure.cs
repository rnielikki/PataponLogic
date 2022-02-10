using PataRoad.Core.Character.Equipments.Weapons;
using UnityEngine;

namespace PataRoad.Core.Character
{
    class CannonStructure : Structure, IAttacker
    {
        private GameObject _weaponInstanceResource;
        private bool _started;

        [SerializeField]
        private Transform _pipe;
        [SerializeField]
        private SpriteRenderer _bullet;
        [SerializeField]
        float _minAngle = 15;
        [SerializeField]
        float _maxAngle = 45;
        float _targetAngle;
        float _currentAngle = 359;
        private bool _changingAngle;
        /// <summary>
        /// Use animator for updating angle.
        /// </summary>
        public bool IsAnimatorUpdatingAngle { get; set; }

        [SerializeField]
        float _minPower = 2;
        [SerializeField]
        float _maxPower = 2.4f;

        [SerializeField]
        private AttackType _attackType;
        public AttackType AttackType => _attackType;

        [SerializeField]
        private ElementalAttackType _elementalAttackType;
        public ElementalAttackType ElementalAttackType => _elementalAttackType;

        [SerializeField]
        private int _damage;

        private void Start()
        {
            _weaponInstanceResource = WeaponInstance.GetResource();
            Stat.DamageMax += _damage;
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
            _targetAngle = 360 - Random.Range(_minAngle, _maxAngle);
            _changingAngle = true;
        }
        private void Attack()
        {
            _animator.SetBool("attacking", false);
            _animator.Play("attacking");
            var instantiated = Instantiate(_weaponInstanceResource);
            instantiated.GetComponent<WeaponInstance>()
                .Initialize(this, _bullet.sprite, _bullet.material, _bullet.gameObject.layer, 2, _bullet.transform)
                .Throw(_minPower, _maxPower);
            StartCoroutine(AnimateAttack());
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
                if (_targetAngle > angle) angle = Mathf.Min(angle + Time.deltaTime * 10, _targetAngle);
                else if (_targetAngle < angle) angle = Mathf.Max(angle - Time.deltaTime * 10, _targetAngle);
                else
                {
                    _changingAngle = false;
                    _animator.SetBool("attacking", true);
                }
                _currentAngle = angle;
            }
            _pipe.transform.eulerAngles = Vector3.forward * _currentAngle;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_started || collision.tag != "Ground") return;
            _started = true;
            StartCoroutine(AnimateAttack());
        }
    }
}
