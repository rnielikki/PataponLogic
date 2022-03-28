namespace PataRoad.Core.Character.Bosses
{
    public class GaruruMSummon : SummonedBoss
    {
        protected override void Chakachaka()
        {
            CharAnimator.Animate("poison");
        }

        protected override void ChargedChakachaka()
        {
            CharAnimator.Animate("poison");
        }
        protected override void Ponpon()
        {
            CharAnimator.Animate("laser");
        }
        protected override void ChargedPonpon()
        {
            CharAnimator.Animate("laser");
        }

        protected override void OnDead()
        {
            //nothing
        }

        protected override void OnStarted()
        {
            //nothing
        }
    }
}