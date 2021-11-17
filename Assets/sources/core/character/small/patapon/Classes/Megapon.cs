namespace PataRoad.Core.Character.Patapons
{
    class Megapon : Patapon
    {
        private void Awake()
        {
            Init();
            Class = ClassType.Megapon;
        }
        void Start()
        {
            AddDefaultModelsToAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-fever", GetAttackMoveModel("attack-fever") },
                    { "defend-charge", GetAttackMoveModel("defend-charge", AttackMoveType.Defend) },
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
            StartAttack(Charged ? "defend-charge" : "defend");
        }
        public override General.IGeneralEffect GetGeneralEffect() => new General.PanPakaponEffect();
    }
}
