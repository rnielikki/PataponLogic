using PataRoad.Core.Character;
using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Map.Levels
{
    class AutoAttack : MonoBehaviour, ICharacter
    {
        private PataponData _pataponData;
        private Animator _animator;
        private Stat _stat;
        private Character.Patapons.PataponsManager _pataponsManager;
        private float _offsetX;
        private bool _walking;
        [SerializeField]
        private AudioClip _startSound;
        [SerializeField]
        private AudioClip _attackSound;
        [SerializeField]
        private AudioClip _dyingSound;
        [SerializeField]
        private Character.Equipments.Weapons.WeaponCannonBullet _weaponBulletTemplate;

        public CharacterAnimator CharAnimator => throw new System.NotImplementedException();

        public DistanceCalculator DistanceCalculator => throw new System.NotImplementedException();
        [SerializeField]
        private Character.Equipments.Weapons.AttackType _attackType;

        public Character.Equipments.Weapons.AttackType AttackType => _attackType;

        public Character.Equipments.Weapons.ElementalAttackType ElementalAttackType => _pataponData.ElementalAttackType;

        public Stat Stat => _stat;

        [SerializeField]
        private AttackTypeResistance _attackTypeResistance;
        [SerializeField]
        private UnityEvent _onDead;
        public AttackTypeResistance AttackTypeResistance => _attackTypeResistance;

        public int CurrentHitPoint { get; private set; }

        public UnityEvent<float> OnDamageTaken => null;

        public StatusEffectManager StatusEffectManager { get; private set; }

        public bool IsDead { get; private set; }

        public float DefaultWorldPosition => _pataponsManager.transform.position.x - _offsetX;

        public Vector2 MovingDirection => Vector2.right;

        public float AttackDistance => 0; //doesn't matter

        public float Sight => CharacterEnvironment.Sight; //doesn't matter, still...

        public float CharacterSize => throw new System.NotImplementedException();

        public bool UseCenterAsAttackTarget => true;

        public UnityEvent OnAfterDeath => null;

        public bool IsAttacking { get; protected set; }

        private bool _ready;

        private void Start()
        {
            _pataponData = GetComponent<PataponData>();
            _pataponData.Init();
            _stat = _pataponData.Stat;

            //without making them infinity it'll throw null exception
            Stat.BoostResistance(Mathf.Infinity);
            Stat.AddCriticalResistance(-Mathf.Infinity);

            StatusEffectManager = null;


            _pataponsManager = FindObjectOfType<Character.Patapons.PataponsManager>();
            transform.position = _pataponsManager.transform.position + (25 * Vector3.left);
            _offsetX = Character.Patapons.PataponEnvironment.GroupDistance * 2;
            _animator = GetComponent<Animator>();
        }
        void WeaponAttack()
        {
            var bullet = Instantiate(_weaponBulletTemplate, transform.root.parent);
            var bulletPos = bullet.transform.position;
            bulletPos.x = transform.position.x;
            bullet.transform.position = bulletPos;
            bullet.Init(this);
        }
        private void Update()
        {
            if (MissionPoint.IsMissionEnd && !_walking) return;
            var targetPosition = _pataponsManager.transform.position + (_offsetX * Vector3.left);
            if (transform.position.x != targetPosition.x && !_walking)
            {
                _walking = true;
                IsAttacking = false;
                _animator.Play("walk");
            }
            else if (transform.position.x == targetPosition.x && _walking)
            {
                _walking = false;
                if (!_ready)
                {
                    _ready = true;
                    IsAttacking = true;
                    GameSound.SpeakManager.Current.Play(_startSound);
                }
                if (!MissionPoint.IsMissionEnd) _animator.Play("attack");
            }
            if (_walking)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, targetPosition, Time.deltaTime * _stat.MovementSpeed);
            }
        }

        public float GetAttackValueOffset()
        {
            if (!Rhythm.Command.TurnCounter.IsOn) return 0;
            return _pataponsManager.FirstPatapon.LastPerfectionRate;
        }
        public float GetDefenceValueOffset() => GetAttackValueOffset();

        public void PlayAttackSound() => GameSound.SpeakManager.Current.Play(_attackSound);

        public void OnAttackHit(Vector2 point, int damage)
        {
            //
        }

        public void OnAttackMiss(Vector2 point)
        {
            //
        }

        public void StopAttacking(bool pause)
        {
            //
        }

        public bool TakeDamage(int damage)
        {
            CurrentHitPoint -= damage;
            return true;
        }

        public void Die()
        {
            IsDead = true;
            GameSound.SpeakManager.Current.Play(_dyingSound);
            _onDead.Invoke();
            Destroy(gameObject);
        }
    }
}
