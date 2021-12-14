using UnityEngine;
using PataRoad.Core.Character.Class;
using PataRoad.Core.Character.Equipments.Weapons;

namespace PataRoad.Core.Character.Hazorons
{
    public class Hazoron : SmallCharacter
    {
        public override float AttackDistance => Weapon.MinAttackDistance + Weapon.WindAttackDistanceOffset * (1 - Map.Weather.WeatherInfo.Current.Wind?.AttackOffsetOnWind ?? 0.5f);

        public override Vector2 MovingDirection => Vector2.left;
        private bool _gotPosition;
        private bool _animatingWalk;
        private int _attackTypeIndex;

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
                ClassData.Attack();
            });
            _attackTypeIndex = (_data as HazoronData).AttackTypeIndex;
        }
        private void Start()
        {
            ClassData.InitLate(Stat);
        }
        protected void InitDistanceFromHead()
        {
            CharacterSize = transform.Find(BodyName + "/Face").GetComponent<CircleCollider2D>().radius + 0.1f;
        }

        public override int GetAttackType() => _attackTypeIndex;
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
            HazoronPositionManager.RemoveHazoron(this);
            GameSound.SpeakManager.Current.Play(CharacterSoundLoader.Current.HazoronSounds.OnDead);
        }
        private void Update()
        {
            if (_gotPosition) return;
            else if (StatusEffectManager.OnStatusEffect && _animatingWalk)
            {
                _animatingWalk = false;
            }
            if (DistanceCalculator.HasAttackTarget())
            {
                DefaultWorldPosition = transform.position.x;
                HazoronPositionManager.AddHazoron(this);
                ClassData.Attack();
                _gotPosition = true;
            }
            else if (!StatusEffectManager.OnStatusEffect)
            {
                if (!_animatingWalk)
                {
                    CharAnimator.Animate("walk");
                    _animatingWalk = true;
                }
                transform.position += (Vector3)(Stat.MovementSpeed * Time.deltaTime * MovingDirection);
            }
        }
    }
}
