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

        public override CharacterSoundsCollection Sounds => CharacterSoundLoader.Current.HazoronSounds;

        private void Awake()
        {
            _stat = _defaultStat;
            OnFever = true;
            Init();
            DefaultWorldPosition = transform.position.x;
            DistanceCalculator = DistanceCalculator.GetHazoronDistanceCalculator(this);
            DistanceManager = gameObject.AddComponent<DistanceManager>();
            StatusEffectManager.SetRecoverAction(() => ClassData.Attack());
            _hazorons.Add(this);
        }
        private void Start()
        {
            ClassData.InitLate();
            ClassData.Attack();
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
        private void OnDestroy()
        {
            _hazorons.Remove(this);
        }
        protected override void BeforeDie()
        {
            GameSound.SpeakManager.Current.Play(CharacterSoundLoader.Current.HazoronSounds.OnDead);
        }
    }
}
