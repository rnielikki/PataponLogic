using Core.Character.Equipment.Weapon;
using UnityEngine;
using System.Linq;

namespace Core.Character.Hazoron
{
    public abstract class Hazoron : SmallCharacter
    {
        //Boss doesn't have default position. Small enemy does.
        private readonly static System.Collections.Generic.List<Hazoron> _hazorons = new System.Collections.Generic.List<Hazoron>();

        /// <summary>
        /// Remember call this on Awake() in inherited class
        /// </summary>
        protected void Init()
        {
            DistanceCalculator = DistanceCalculator.GetHazoronDistanceCalculator(this, _rangeForAttack);
            CharAnimator = new CharacterAnimator(GetComponent<Animator>());
            Stat = DefaultStat;
            CurrentHitPoint = Stat.HitPoint;
            Weapon = GetComponentInChildren<WeaponObject>();
            AttackMoveData = new HazoronAttackMoveData(this);
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
            return _hazorons.Min(h => h.AttackMoveData.DefaultWorldPosition);
        }

        protected void InitDistanceFromHead(float attackDistance)
        {
            AttackDistance = attackDistance;
            CharacterSize = transform.Find("Patapon-body/Face").GetComponent<CircleCollider2D>().radius + 0.1f;
        }

        public override int GetCurrentDamage()
        {
            return Random.Range(Stat.DamageMin, Stat.DamageMax);
        }
        private void OnDestroy()
        {
            _hazorons.Remove(this);
        }
    }
}
