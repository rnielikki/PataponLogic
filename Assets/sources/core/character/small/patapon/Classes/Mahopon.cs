namespace PataRoad.Core.Character.Patapons
{
    class Mahopon : Patapon
    {
        private void Awake()
        {
            Init();
            Class = ClassType.Mahopon;
        }
        void Start()
        {
            AddDefaultModelsToAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-charge", GetAttackMoveModel("attack-charge") },
                }
                );
            WeaponLoadTest("Staff", 1);
        }
        protected override void Attack()
        {
            if (!Charged)
            {
                base.Attack();
            }
            else
            {
                StartAttack("attack-charge");
            }
        }
        protected override void Charge() => ChargeWithoutMoving();
    }
}
