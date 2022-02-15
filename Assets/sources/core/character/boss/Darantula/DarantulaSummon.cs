namespace PataRoad.Core.Character.Bosses
{
    class DarantulaSummon : SummonedBoss
    {
        protected override void Chakachaka()
        {
            CharAnimator.Animate("poison");
        }

        protected override void ChargedChakachaka()
        {
            CharAnimator.Animate("poison");
        }

        protected override void ChargedPonpon()
        {
            CharAnimator.Animate("tailslide");
        }

        protected override void Ponpon()
        {
            CharAnimator.Animate("tailwhip");
        }
    }
}
