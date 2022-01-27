namespace PataRoad.Core.Character.Bosses
{
    class MochichichiSummon : SummonedBoss
    {
        protected override void Ponpon()
        {
            CharAnimator.Animate("peck");
        }
        protected override void ChargedPonpon()
        {
            CharAnimator.Animate("slam");
        }
        protected override void Chakachaka()
        {
            CharAnimator.Animate("fart");
        }
        protected override void ChargedChakachaka()
        {
            CharAnimator.Animate("tornado");
        }
        private void Awake()
        {
            Init();
        }
    }
}
