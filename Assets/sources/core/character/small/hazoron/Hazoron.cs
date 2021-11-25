using UnityEngine;
using System.Linq;

namespace PataRoad.Core.Character.Hazorons
{
    public abstract class Hazoron : SmallCharacter
    {
        //Boss doesn't have default position. Small enemy does.
        private readonly static System.Collections.Generic.List<Hazoron> _hazorons = new System.Collections.Generic.List<Hazoron>();
        public override float AttackDistance => Weapon.MinAttackDistance + Weapon.WindAttackDistanceOffset * (1 - Map.Weather.WeatherInfo.Wind?.AttackOffsetOnWind ?? 0.5f);

        public override Vector2 MovingDirection => Vector2.left;
        protected Stat _stat;
        public override Stat Stat => _stat;

        public override CharacterSoundsCollection Sounds => CharacterSoundLoader.Current.HazoronSounds;

        /// <summary>
        /// Remember call this on Awake() in inherited class
        /// </summary>
        protected override void Init()
        {
            _stat = _defaultStat;
            base.Init();
            DefaultWorldPosition = transform.position.x;
            DistanceCalculator = DistanceCalculator.GetHazoronDistanceCalculator(this);
            AttackMoveData = new HazoronAttackMoveData(this);
            //Save. this can be changed later.
            StatusEffectManager.SetRecoverAction(() => StartAttack("attack-fever"));
        }

        protected override AttackMoveController SetAttackMoveController()
        {
            AttackMoveData = new HazoronAttackMoveData(this);
            _hazorons.Add(this);
            return base.SetAttackMoveController();
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
