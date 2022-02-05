namespace PataRoad.Core.Character.Bosses
{
    class DogaeenEnemy : EnemyBoss
    {
        private void Awake()
        {
            Init();
            CharacterSize = 7;
        }

        protected override float CalculateAttack()
        {
            BossTurnManager
                .SetOneAction("slam");
            return 0.5f;
        }
    }
}
