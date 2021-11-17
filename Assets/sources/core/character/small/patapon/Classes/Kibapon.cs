namespace PataRoad.Core.Character.Patapons
{
    public class Kibapon : Patapon
    {
        private void Awake()
        {
            Init();
            Class = ClassType.Kibapon;
        }
        void Start()
        {
            AddDefaultModelsToAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-fever", GetAttackMoveModel("attack-fever", AttackMoveType.Rush, 2) },
                    { "defend-fever", GetAttackMoveModel("defend-fever", AttackMoveType.Defend).SetAlwaysAnimate() },
                }
                );
        }
        protected override void Attack()
        {
            if (!OnFever && !Charged)
            {
                base.Attack();
            }
            else
            {
                StartAttack("attack-fever");
            }
        }
        protected override void Defend()
        {
            if (!OnFever && !Charged)
            {
                StartAttack("defend");
            }
            else
            {
                StartAttack("defend-fever");
            }
        }
        protected override void Charge()
        {
            base.Charge();
            DistanceManager.MoveToInitialPlace(Stat.MovementSpeed);
        }
        public override General.IGeneralEffect GetGeneralEffect() => new General.HataponKibaEffect();
    }
}
