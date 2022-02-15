namespace PataRoad.Core.Character.Bosses
{
    class DarantulaEnemy : EnemyBossBehaviour
    {
        public void SetAbsorbHit()
        {
            _boss.CharAnimator.Animate("absorbing");
        }

        protected override (string action, float distance) GetNextBehaviour()
        {
            throw new System.NotImplementedException();
        }

        protected override string GetNextBehaviourOnIce()
        {
            throw new System.NotImplementedException();
        }

        protected override void Init()
        {
            throw new System.NotImplementedException();
        }
    }
}
