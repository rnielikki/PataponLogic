namespace PataRoad.Core.Character.Bosses
{
    class DogaeenSummon : SummonedBoss
    {
        protected override void Chakachaka()
        {
            CharAnimator.Animate("laser");
        }
        protected override void ChargedChakachaka()
        {
            CharAnimator.Animate("repel");
        }
        protected override void Ponpon()
        {
            CharAnimator.Animate("slam");
        }
        protected override void ChargedPonpon()
        {
            CharAnimator.Animate("bodyslam");
        }
    }
}
