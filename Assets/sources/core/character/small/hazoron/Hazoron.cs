using PataRoad.Core.Character.Class;
using PataRoad.Core.Character.Equipments.Weapons;
using UnityEngine;

namespace PataRoad.Core.Character.Hazorons
{
    public class Hazoron : SmallCharacter
    {
        public override float AttackDistance => Weapon.GetAttackDistance();

        [SerializeField]
        UnityEngine.Events.UnityEvent _onBeforeDeath;
        [SerializeField]
        bool _inverseDirection;
        private bool _melee;
        public override Vector2 MovingDirection => _inverseDirection ? Vector2.right : Vector2.left;
        private bool _gotPosition;
        private bool _fullyGotPosition;
        private bool _animatingWalk;
        [SerializeField]
        protected int _attackTypeIndex;
        public override int AttackTypeIndex => _attackTypeIndex;
        [Header("Position status")]
        [SerializeField]
        private bool _isOnTower;

        public override bool IsFixedPosition => _isOnTower;

        [Header("Attack status")]
        [SerializeField]
        protected bool _charged;
        [SerializeField]
        protected bool _defend;

        [SerializeField]
        protected bool _isDarkOne;
        public bool IsDarkOne => _isDarkOne;
        [SerializeField]
        private bool _manualDeath;

        private bool _isReady;

        public override CharacterSoundsCollection Sounds => _isDarkOne ?
            CharacterSoundLoader.Current.DarkOneSounds : CharacterSoundLoader.Current.HazoronSounds;
        private float _maxAttackSight;
        private Stat _realStat;

        private void Awake()
        {
            Init();
            if (_isOnTower) _realStat.AddKnockbackResistance(Mathf.Infinity);
            _melee = ClassData.IsMeleeUnit;

            StatusEffectManager.AddRecoverAction((_) =>
            {
                if (IsDead) return;
                IsAttacking = false;
                if (IsFlyingUnit)
                {
                    (ClassData as ToriClassData)?.FlyUp();
                    _melee = true;
                }
                if (_gotPosition)
                {
                    Attack();
                }
            });
            foreach (var modifier in GetComponents<Levels.IHazoronStatModifier>())
            {
                modifier.SetModifyTarget(_realStat);
            }
        }

        private void Start()
        {
            CurrentHitPoint = _realStat.HitPoint;
            ClassData.InitLate(_realStat);
            _maxAttackSight = IsMeleeUnit ? CharacterEnvironment.MaxAttackDistance : Sight;

            _isReady = true;
        }
        protected override void Init()
        {
            Charged = _charged;
            OnFever = true;
            DefaultWorldPosition = transform.position.x;

            base.Init();

            _realStat = _data.Stat;
            Stat = _realStat;

            DistanceCalculator = _isDarkOne
                ? DistanceCalculator.GetNonPataHazoDistanceCalculator(this)
                : DistanceCalculator.GetHazoronDistanceCalculator(this);

            DistanceManager = gameObject.AddComponent<DistanceManager>();
            DistanceManager.DistanceCalculator = DistanceCalculator;
            _attackTypeIndex = (_data as HazoronData).AttackTypeIndex;
        }
        protected void Attack()
        {
            if (!_isReady || IsAttacking || IsDead) return;
            //reset stat that can be modified by performcommandaciton()
            Stat.SetValuesTo(_realStat);
            _animatingWalk = false;
            IsAttacking = true;
            if (_defend)
            {
                ClassData.Defend();
                ClassData.PerformCommandAction(Rhythm.Command.CommandSong.Chakachaka);
            }
            else
            {
                ClassData.Attack();
                ClassData.PerformCommandAction(Rhythm.Command.CommandSong.Ponpon);
            }
        }
        public void ChangeAttackStatus()
        {
            _defend = !_defend;
            if (IsAttacking)
            {
                //reset attack status.
                StopAttacking(false);
                Attack();
            }
        }
        public override bool TakeDamage(int damage)
        {
            if (!_manualDeath) base.TakeDamage(damage);
            return !_manualDeath;
        }
        public override float GetAttackValueOffset()
        {
            return Random.Range(0, 1f);
        }
        public override float GetDefenceValueOffset()
        {
            return Random.Range(0, 1f);
        }
        protected override void BeforeDie()
        {
            _onBeforeDeath?.Invoke();
            HazoronPositionManager.Current.RemoveHazoron(this);
            base.BeforeDie();
        }
        private bool IsInPataponsSight(float pos)
        {
            return pos > Patapons.PataponsManager.Current.transform.position.x
                &&
                pos < Patapons.PataponsManager.Current.transform.position.x + CharacterEnvironment.Sight;
        }
        private void Register()
        {
            HazoronPositionManager.Current.AddHazoron(this);
            _fullyGotPosition = true;
        }
        public override void StopAttacking(bool pause)
        {
            IsAttacking = false;
            base.StopAttacking(pause);
        }

        private void Update()
        {
            if ((_fullyGotPosition && _isOnTower) || !_isReady) return;
            if (!StatusEffectManager.CanContinue)
            {
                _animatingWalk = false;
                return;
            }
            if (_isOnTower)
            {
                if (DistanceCalculator.GetTargetOnSight(Sight) != null)
                {
                    _gotPosition = true;
                    _fullyGotPosition = true;
                    Attack();
                }
                return;
            }
            else if (_fullyGotPosition)
            {
                var hasEnemyOnSight = DistanceCalculator.HasSightFromWorldDefault(_maxAttackSight, _melee);
                if (IsAttacking && !hasEnemyOnSight)
                {
                    StopAttacking(false);
                    if (!_animatingWalk)
                    {
                        _animatingWalk = true;
                        CharAnimator.Animate("walk");
                    }
                }
                else if (!IsAttacking && hasEnemyOnSight)
                {
                    _animatingWalk = false;
                    Attack();
                }
                if (_animatingWalk)
                {
                    transform.position
                        = Vector3.MoveTowards(
                            transform.position, DefaultWorldPosition * Vector3.right, Stat.MovementSpeed * Time.deltaTime);
                    if (transform.position.x == DefaultWorldPosition)
                    {
                        CharAnimator.Animate("Idle");
                        StopAttacking(false); //Idle status doesn't stop attacking.
                        _animatingWalk = false;
                    }
                }
                return;
            }
            else if (_gotPosition)
            {
                if (IsInPataponsSight(transform.position.x)) Register();
                else if (!ClassData.IsInAttackDistance())
                {
                    StopAttacking(false);
                    _gotPosition = false;
                }
                else DefaultWorldPosition = transform.position.x;
            }
            if (ClassData.IsInAttackDistance())
            {
                DefaultWorldPosition = transform.position.x;
                Attack();
                _gotPosition = true;
                if (IsInPataponsSight(transform.position.x)) Register();
            }
            else if (!StatusEffectManager.IsOnStatusEffect
                && DistanceCalculator.HasSightFromWorldDefault(_maxAttackSight, _melee))
            {
                if (!_animatingWalk)
                {
                    CharAnimator.Animate("walk");
                    _animatingWalk = true;
                }
                var oldpos = transform.position.x;
                transform.position =
                    DistanceCalculator.GetSafeForwardPosition(
                        transform.position.x + (Stat.MovementSpeed * Time.deltaTime * MovingDirection.x))
                    * Vector3.right;
                //can't go forward
                if (DistanceCalculator.IsInTargetRange(oldpos, 0.01f))
                {
                    Attack();
                    _gotPosition = true;
                    if (IsInPataponsSight(transform.position.x)) Register();
                }
            }
        }
    }
}
