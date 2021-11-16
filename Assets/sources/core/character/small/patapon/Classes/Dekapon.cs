namespace PataRoad.Core.Character.Patapons
{
    public class Dekapon : Patapon
    {
        private void Awake()
        {
            Init();
            Class = ClassType.Dekapon;
        }
        void Start()
        {
            AddDefaultModelsToAttackMoveController();
        }
        protected override void Attack()
        {
            if (!Charged)
            {
                base.Attack();
            }
            else
            {
                CharAnimator.Animate("attack-charge");
                DistanceManager.MoveZero(1.6f);
            }
        }
        public void TumbleAttack() => StatusEffectManager.TumbleAttack();
    }
}
