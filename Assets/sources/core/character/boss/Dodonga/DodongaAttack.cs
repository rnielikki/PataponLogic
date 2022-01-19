using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class DodongaAttack : BossAttackData
    {
        [SerializeField]
        private BossAttackParticle Fire;
        [SerializeField]
        private BossAttackCollision Headbutt;
        [SerializeField]
        private DodongaEatingComponent EatingMouth;

        internal void AnimateFireBefore()
        {
            CharAnimator.Animate("fire-before");
        }
        internal void AnimateFire()
        {
            CharAnimator.Animate("fire");
        }
        internal void AnimateHeadbuttBefore()
        {
            CharAnimator.Animate("headbutt-before");
        }
        internal void AnimateHeadbutt()
        {
            CharAnimator.Animate("headbutt");
        }
        internal void AnimateEatBefore()
        {
            CharAnimator.Animate("eat-before");
        }
        internal void AnimateEat()
        {
            CharAnimator.Animate("eat");
        }
        internal void AnimateGrowl()
        {
            CharAnimator.Animate("growl");
        }

        public void FireAttack()
        {
            _boss.AttackType = Equipments.Weapons.AttackType.Magic;
            _boss.ElementalAttackType = Equipments.Weapons.ElementalAttackType.Fire;
            Fire.Attack();
        }
        public void HeadbuttAttack()
        {
            _boss.AttackType = Equipments.Weapons.AttackType.Crush;
            _boss.ElementalAttackType = Equipments.Weapons.ElementalAttackType.Neutral;
            Headbutt.Attack();
        }
        public void StopHeadbuttAttack()
        {
            Headbutt.StopAttacking();
            _boss.AttackType = Equipments.Weapons.AttackType.Neutral;
            _boss.ElementalAttackType = Equipments.Weapons.ElementalAttackType.Neutral;
        }
        public void EatingAttack()
        {
            EatingMouth.Attack();
        }
        public void StopEatingAttack()
        {
            EatingMouth.StopAttacking();
        }
        public void GrowlAttack()
        {
            foreach (var target in _boss.DistanceCalculator.GetAllAbsoluteTargetsOnFront())
            {
                Equipments.Logic.DamageCalculator.DealDamage(_boss, _stat, target.gameObject, target.transform.position);
            }
        }
        public override void StopAllAttacking()
        {
            Headbutt.StopAttacking();
            EatingMouth.StopAttacking();
        }
        internal override void UpdateStatForBoss(int level)
        {
            var sqrt = Mathf.Sqrt(level);
            _stat.MultipleDamage(sqrt);
            _stat.DefenceMin += sqrt;
            _stat.DefenceMax += sqrt;
            _boss.SetMaximumHitPoint(Mathf.RoundToInt(_stat.HitPoint * sqrt));
            _stat.CriticalResistance += level * 0.05f;
            _stat.StaggerResistance += level * 0.05f;
            _stat.KnockbackResistance += level * 0.05f;
            _stat.FireResistance += level * 0.03f;
            _stat.IceResistance += level * 0.03f;
            _stat.SleepResistance += level * 0.03f;
        }
    }
}
