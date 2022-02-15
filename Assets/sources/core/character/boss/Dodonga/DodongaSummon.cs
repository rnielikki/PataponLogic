namespace PataRoad.Core.Character.Bosses
{
    internal class DodongaSummon : SummonedBoss
    {
        protected override void Chakachaka()
        {
            CharAnimator.Animate("fire");
        }

        protected override void ChargedChakachaka()
        {
            CharAnimator.Animate("fire");
        }

        protected override void ChargedPonpon()
        {
            CharAnimator.Animate("headbutt");
        }

        protected override void Ponpon()
        {
            CharAnimator.Animate("headbutt");
        }

        private void Awake()
        {
            Init();
        }
    }
}
