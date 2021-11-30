using UnityEngine;
using PataRoad.Core.Character.Class;

namespace PataRoad.Core.Character.Hazorons
{
    public class Hazoron : SmallCharacter
    {
        public override float AttackDistance => Weapon.MinAttackDistance + Weapon.WindAttackDistanceOffset * (1 - Map.Weather.WeatherInfo.Wind?.AttackOffsetOnWind ?? 0.5f);

        public override Vector2 MovingDirection => Vector2.left;
        protected Stat _stat;
        public override Stat Stat => _stat;
        private bool _gotPosition;
        private bool _animatingWalk;

        [SerializeField]
        Items.EquipmentData _weaponData;
        [SerializeField]
        Items.EquipmentData _protectorData;

        public override CharacterSoundsCollection Sounds => CharacterSoundLoader.Current.HazoronSounds;

        private void Awake()
        {
            _stat = _defaultStat;
            OnFever = true;
            Init(_weaponData, _protectorData);
            DistanceCalculator = DistanceCalculator.GetHazoronDistanceCalculator(this);
            DistanceManager = gameObject.AddComponent<DistanceManager>();
            StatusEffectManager.SetRecoverAction(() => ClassData.Attack());
        }
        private void Start()
        {
            ClassData.InitLate();
        }
        protected void InitDistanceFromHead()
        {
            CharacterSize = transform.Find(BodyName + "/Face").GetComponent<CircleCollider2D>().radius + 0.1f;
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
        private void OnValidate()
        {
            if (_weaponData != null && _weaponData.Type != Equipments.EquipmentType.Weapon)
            {
                throw new System.ArgumentException("Weapon data should be type of weapon but it's " + _weaponData.Type);
            }
            if (_protectorData != null && _protectorData.Type != Equipments.EquipmentType.Protector)
            {
                throw new System.ArgumentException("Protector should be type of protector but it's " + _protectorData.Type);
            }
        }
    }
}
