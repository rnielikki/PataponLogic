namespace PataRoad.Core.Character.Bosses
{
    public class GaruruEnemyBoss : EnemyBoss
    {
        private GaruruAttack _garuruAttack;
        private void Start()
        {
            _garuruAttack = BossAttackData as GaruruAttack;
        }
        protected override void StartMovingBack()
        {
            _garuruAttack.ChangeForm();
        }
        internal void ChangedForm()
        {
            _movingBackQueued = false;
        }
    }
}