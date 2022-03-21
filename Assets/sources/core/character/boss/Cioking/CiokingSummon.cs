namespace PataRoad.Core.Character.Bosses
{
    public class CiokingSummon : SummonedBoss
    {
        protected override void Chakachaka()
        {
            CharAnimator.Animate("sleep-bubble");
        }

        protected override void ChargedChakachaka()
        {
            CharAnimator.Animate("sleep-bubble");
        }
        protected override void Ponpon()
        {
            CharAnimator.Animate("slash");
        }
        protected override void ChargedPonpon()
        {
            CharAnimator.Animate("slash");
        }
        protected override void OnStarted()
        {
            //no weather change
        }
        protected override void OnDead()
        {
            //no weather change
        }
    }
}