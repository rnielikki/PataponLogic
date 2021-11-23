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

        internal void AnimateFire()
        {
            CharAnimator.Animate("fire-before");
        }
        internal void AnimateHeadbutt()
        {
            CharAnimator.Animate("headbutt-before");
        }
        internal void AnimateEat()
        {
            CharAnimator.Animate("eat-before");
        }

        public void FireAttack()
        {
            Fire.Attack();
        }
        public void HeadbuttAttack()
        {
            Headbutt.Attack();
        }
        public void StopHeadbuttAttack()
        {
            Headbutt.StopAttacking();
        }
        public void EatingAttack()
        {
            EatingMouth.Attack();
        }
        public void StopEatingAttack()
        {
            EatingMouth.StopAttacking();
        }
    }
}
