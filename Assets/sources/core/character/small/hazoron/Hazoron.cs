using UnityEngine;
using PataRoad.Core.Character.Class;
using PataRoad.Core.Character.Equipments.Weapons;

namespace PataRoad.Core.Character.Hazorons
{
    public class Hazoron : SmallCharacter
    {
        public override float AttackDistance => Weapon.GetAttackDistance();

        public override Vector2 MovingDirection => Vector2.left;
        private bool _gotPosition;
        private bool _animatingWalk;
        private int _attackTypeIndex;
        public override int AttackTypeIndex => _attackTypeIndex;
        [SerializeField]
        private bool _isOnTower;
        public override bool IsFixedPosition => _isOnTower;
        [SerializeField]
        private bool _charged;
        [SerializeField]
        private bool _defineCommandAction;
        [SerializeField]
        [Header("Valid only if Define Command Action is true")]
        private Rhythm.Command.CommandSong _commandAction;

        [SerializeField]
        private bool _isDarkOne;

        public override CharacterSoundsCollection Sounds => _isDarkOne ?
            CharacterSoundLoader.Current.DarkOneSounds : CharacterSoundLoader.Current.HazoronSounds;

        private void Awake()
        {
            Charged = _charged;
            OnFever = true;
            Init();
            Stat = _data.Stat;
            DistanceCalculator = _isDarkOne ? DistanceCalculator.GetNonPataHazoDistanceCalculator(this) : DistanceCalculator.GetHazoronDistanceCalculator(this);
            DistanceManager = gameObject.AddComponent<DistanceManager>();

            StatusEffectManager.AddRecoverAction(() =>
            {
                if (IsFlyingUnit)
                {
                    (ClassData as ToriClassData)?.FlyUp();
                }
                else if (_gotPosition)
                {
                    if (!_defineCommandAction) ClassData.Attack();
                    else ClassData.PerformCommandAction(_commandAction);
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
            DefaultWorldPosition = transform.position.x;

            CurrentHitPoint = Stat.HitPoint;
            ClassData.InitLate(Stat);
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
            base.BeforeDie();
            HazoronPositionManager.Current.RemoveHazoron(this);
        }
        private void Update()
        {
            if (_gotPosition) return;
            else if (_isOnTower)
            {
                if (DistanceCalculator.GetTargetOnSight(CharacterEnvironment.Sight) != null)
                {
                    _gotPosition = true;
                    if (!_defineCommandAction) ClassData.Attack();
                    else ClassData.PerformCommandAction(_commandAction);
                }
                return;
            }
            else if (StatusEffectManager.IsOnStatusEffect && _animatingWalk)
            {
                _animatingWalk = false;
            }

            if (ClassData.IsInAttackDistance())
            {
                DefaultWorldPosition = transform.position.x;
                HazoronPositionManager.Current.AddHazoron(this);
                ClassData.Attack();
                _gotPosition = true;
                return;
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
            DefaultWorldPosition = transform.position.x;
        }
    }
}
