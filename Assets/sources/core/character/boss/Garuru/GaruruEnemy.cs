namespace PataRoad.Core.Character.Bosses
{
    public class GaruruEnemy : EnemyBossBehaviour, IAbsorbableBossBehaviour
    {
        public void Heal(int amount)
        {
            //no heal, just burn
        }

        public void SetAbsorbHit()
        {
            UseCustomDataPosition = true;
            Boss.CharAnimator.Animate("burning");
        }
        public void StopAbsorbing()
        {
            UseCustomDataPosition = false;
        }

        protected override BossAttackMoveSegment GetNextBehaviour()
        {
            throw new System.NotImplementedException();
        }
        protected override string GetNextBehaviourOnIce()
        {
            //ice attack is impossible but...
            return GetNextBehaviour().Action;
        }
    }
}