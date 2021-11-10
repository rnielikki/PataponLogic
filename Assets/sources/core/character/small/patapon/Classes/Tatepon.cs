namespace PataRoad.Core.Character.Patapons
{
    public class Tatepon : Patapon
    {
        private void Awake()
        {
            Init();
            Class = ClassType.Tatepon;
        }
        void Start()
        {
            SetAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack", GetAttackMoveModel("attack") },
                    { "attack-charge", GetAttackMoveModel("attack-charge", AttackMoveType.Rush, movingSpeed: 1.2f) },
                }
                );
        }
        protected override void Attack()
        {
            if (Charged)
            {
                StartAttack("attack-charge");
            }
            else base.Attack();
        }

        protected override void Defend()
        {
            if (!OnFever && !Charged)
            {
                CharAnimator.Animate("defend");
            }
            else
            {
                CharAnimator.Animate("defend-fever");
            }
            DistanceManager.MoveTo(0.75f, Stat.MovementSpeed);
        }
        protected override void Charge()
        {
            base.Charge();
            DistanceManager.MoveToInitialPlace(Stat.MovementSpeed);
        }
    }
}
