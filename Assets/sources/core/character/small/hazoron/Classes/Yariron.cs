namespace PataRoad.Core.Character.Hazorons
{
    public class Yariron : Hazoron
    {
        private void Awake()
        {
            Init();
        }
        private void Start()
        {
            SetAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-fever", GetAttackMoveModel("attack-fever") },
                }
                );
            StartAttack("attack-fever");
        }
    }
}
