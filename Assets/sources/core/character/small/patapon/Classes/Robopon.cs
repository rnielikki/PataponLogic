namespace PataRoad.Core.Character.Patapons
{
    public class Robopon : Patapon
    {
        private void Awake()
        {
            Init();
            Class = ClassType.Robopon;
        }
        void Start()
        {
            AddDefaultModelsToAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-charge", GetAttackMoveModel("attack-charge", attackDistance: 4.5f) },
                }
                );
            WeaponLoadTest("Helm/1");
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
