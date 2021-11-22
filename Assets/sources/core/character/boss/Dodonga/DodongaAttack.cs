using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class DodongaAttack : BossAttackData
    {
        [SerializeField]
        private ParticleSystem Fire;

        internal void AnimateFire()
        {
            CharAnimator.Animate("fire-before");
        }

        public void FireAttack()
        {
            Fire.Play();
        }
    }
}
