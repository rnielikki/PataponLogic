using UnityEngine;
using System.Linq;
using PataRoad.Core.Character.Class;

namespace PataRoad.Core.Character.Hazorons
{
    public class Hazoron : SmallCharacter
    {
        //Boss doesn't have default position. Small enemy does.
        private readonly static System.Collections.Generic.List<Hazoron> _hazorons = new System.Collections.Generic.List<Hazoron>();
        public override float AttackDistance => Weapon.MinAttackDistance + Weapon.WindAttackDistanceOffset * (1 - Map.Weather.WeatherInfo.Wind?.AttackOffsetOnWind ?? 0.5f);

        public override Vector2 MovingDirection => Vector2.left;
        protected Stat _stat;
        public override Stat Stat => _stat;
        private bool _gotPosition;
        private bool _animatingWalk;

        public override CharacterSoundsCollection Sounds => CharacterSoundLoader.Current.HazoronSounds;

        private void Awake()
        {
            _stat = _defaultStat;
            OnFever = true;
            Init();
            DistanceCalculator = DistanceCalculator.GetHazoronDistanceCalculator(this);
            DistanceManager = gameObject.AddComponent<DistanceManager>();
            StatusEffectManager.SetRecoverAction(() => ClassData.Attack());
        }
        private void Start()
        {
            ClassData.InitLate();
        }
        public static float GetClosestHazoronPosition()
        {
            if (_hazorons.Count == 0) return Mathf.Infinity;
            return _hazorons.Min(h => h.DefaultWorldPosition);
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
            _hazorons.Remove(this);
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
                _hazorons.Add(this);
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
