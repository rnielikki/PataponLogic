namespace PataRoad.Core.Character.Bosses
{
    class GanodiasSummon : SummonedBoss
    {
        protected override void Ponpon()
        {
            CharAnimator.Animate("mace");
        }

        protected override void ChargedPonpon()
        {
            CharAnimator.Animate("mace");
        }
        protected override void Chakachaka()
        {
            CharAnimator.Animate("bullet");
        }
        protected override void ChargedChakachaka()
        {
            CharAnimator.Animate("bullet");
        }
        protected override void OnStarted()
        {
        }
        protected override void OnDead()
        {
        }
    }
}
