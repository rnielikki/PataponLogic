namespace PataRoad.Core.Character.Bosses
{
    public class DokaknelSummon : SummonedBoss
    {
        protected override void Chakachaka()
        {
            CharAnimator.Animate("fire");
        }
        protected override void ChargedChakachaka()
        {
            CharAnimator.Animate("earthquake");
        }
        protected override void Ponpon()
        {
            CharAnimator.Animate("slam");
        }
        protected override void ChargedPonpon()
        {
            CharAnimator.Animate("wheel");
        }

        protected override void OnDead()
        {
            //boiler
        }

        protected override void OnStarted()
        {
            //plate
        }
    }
}