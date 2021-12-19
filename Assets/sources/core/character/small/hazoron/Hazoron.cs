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

        public override CharacterSoundsCollection Sounds => CharacterSoundLoader.Current.HazoronSounds;

        private void Awake()
        {
            OnFever = true;
            Init();
            Stat = _data.Stat;
            DistanceCalculator = DistanceCalculator.GetHazoronDistanceCalculator(this);
            DistanceManager = gameObject.AddComponent<DistanceManager>();

            StatusEffectManager.SetRecoverAction(() =>
            {
                if (IsFlyingUnit)
                {
                    (ClassData as ToriClassData)?.FlyUp();
                }
                if (_gotPosition) ClassData.Attack();
            });
            _attackTypeIndex = (_data as HazoronData).AttackTypeIndex;
        }
        private void Start()
        {
            DefaultWorldPosition = transform.position.x;
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
            HazoronPositionManager.Current.RemoveHazoron(this);
            GameSound.SpeakManager.Current.Play(CharacterSoundLoader.Current.HazoronSounds.OnDead);
        }
        private void Update()
        {
            if (_gotPosition) return;
            else if (StatusEffectManager.OnStatusEffect && _animatingWalk)
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
            else if (!StatusEffectManager.OnStatusEffect)
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
