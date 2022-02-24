using UnityEngine;
using PataRoad.Core.Character.Class;
using PataRoad.Core.Character.Equipments.Weapons;

namespace PataRoad.Core.Character.Hazorons
{
    public class Hazoron : SmallCharacter
    {
        public override float AttackDistance => Weapon.GetAttackDistance();

        [SerializeField]
        UnityEngine.Events.UnityEvent _onBeforeDeath;
        [SerializeField]
        bool _inverseDirection;
        public override Vector2 MovingDirection => _inverseDirection ? Vector2.right : Vector2.left;
        private bool _gotPosition;
        private bool _fullyGotPosition;
        private bool _animatingWalk;
        private int _attackTypeIndex;
        public override int AttackTypeIndex => _attackTypeIndex;
        [Header("Position status")]
        [SerializeField]
        private bool _isOnTower;
        public override bool IsFixedPosition => _isOnTower;

        [Header("Attack status")]
        [SerializeField]
        private bool _charged;
        [SerializeField]
        private bool _defend;
        [SerializeField]
        private bool _doNothing;

        [SerializeField]
        private bool _isDarkOne;
        public bool IsDarkOne => _isDarkOne;
        [SerializeField]
        private bool _manualDeath;

        private bool _isReady;
        private bool _isAttacking;

        public override CharacterSoundsCollection Sounds => _isDarkOne ?
            CharacterSoundLoader.Current.DarkOneSounds : CharacterSoundLoader.Current.HazoronSounds;

        private void Awake()
        {
            Charged = _charged;
            OnFever = true;
            DefaultWorldPosition = transform.position.x;
            Init();
            Stat = _data.Stat;
            DistanceCalculator = _isDarkOne ? DistanceCalculator.GetNonPataHazoDistanceCalculator(this) : DistanceCalculator.GetHazoronDistanceCalculator(this);
            DistanceManager = gameObject.AddComponent<DistanceManager>();
            DistanceManager.DistanceCalculator = DistanceCalculator;
            if (_isOnTower) Stat.KnockbackResistance = Mathf.Infinity;

            if (_doNothing) _isAttacking = true;
            StatusEffectManager.AddRecoverAction(() =>
            {
                if (IsFlyingUnit)
                {
                    (ClassData as ToriClassData)?.FlyUp();
                }
                if (_gotPosition && !_doNothing)
                {
                    Attack();
                }
            });
            _attackTypeIndex = (_data as HazoronData).AttackTypeIndex;
            foreach (var modifier in GetComponents<Levels.IHazoronStatModifier>())
            {
                modifier.SetModifyTarget(Stat);
            }
        }

        private void Start()
        {
            CurrentHitPoint = Stat.HitPoint;
            ClassData.InitLate(Stat);

            _isReady = true;
        }
        private void Attack()
        {
            if (!_isReady || _isAttacking) return;
            _isAttacking = true;
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
        public override void TakeDamage(int damage)
        {
            if (!_manualDeath) base.TakeDamage(damage);
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
            _isAttacking = false;
            base.StopAttacking(pause);
        }

        private void Update()
        {
            if (_fullyGotPosition || !_isReady) return;
            else if (_isOnTower)
            {
                if (DistanceCalculator.GetTargetOnSight(CharacterEnvironment.Sight) != null)
                {
                    _gotPosition = true;
                    _fullyGotPosition = true;
                    Attack();
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
            else if (StatusEffectManager.IsOnStatusEffect && _animatingWalk)
            {
                _animatingWalk = false;
            }
            if (ClassData.IsInAttackDistance())
            {
                DefaultWorldPosition = transform.position.x;
                Attack();
                _gotPosition = true;
                if (IsInPataponsSight(transform.position.x)) Register();
            }
            else if (!StatusEffectManager.IsOnStatusEffect
                && DistanceCalculator.GetTargetOnSight(CharacterEnvironment.Sight) != null)
            {
                if (!_animatingWalk)
                {
                    CharAnimator.Animate("walk");
                    _animatingWalk = true;
                }
                transform.position =
                    DistanceCalculator.GetSafeForwardPosition(
                        transform.position.x + Stat.MovementSpeed * Time.deltaTime * MovingDirection.x)
                    * Vector3.right;
            }
        }
    }
}
