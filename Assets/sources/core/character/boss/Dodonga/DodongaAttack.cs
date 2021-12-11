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

        public void FireAttack()
        {
            _boss.AttackType = Equipments.Weapons.AttackType.Magic;
            Fire.Attack();
        }
        public void HeadbuttAttack()
        {
            _boss.AttackType = Equipments.Weapons.AttackType.Crush;
            Headbutt.Attack();
        }
        public void StopHeadbuttAttack()
        {
            Headbutt.StopAttacking();
            _boss.AttackType = Equipments.Weapons.AttackType.Neutral;
        }
        public void EatingAttack()
        {
            EatingMouth.Attack();
        }
        public void StopEatingAttack()
        {
            EatingMouth.StopAttacking();
        }
        public override void StopAllAttacking()
        {
            Headbutt.StopAttacking();
            EatingMouth.StopAttacking();
        }
    }
}
