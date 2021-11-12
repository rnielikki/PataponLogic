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
            WeaponLoadTest("Rarepons/Funmyaga");
            WeaponLoadTest("Rarepons/Gekolo");
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
    }
}
