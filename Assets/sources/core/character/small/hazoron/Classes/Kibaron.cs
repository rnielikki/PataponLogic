using PataRoad.Core.Character.Patapons;
using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Hazorons
{
    public class Kibaron : Hazoron
    {
        private void Awake()
        {
            Init();
            Class = ClassType.Kibapon;
        }
        private void Start()
        {
            SetAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-fever", GetAttackMoveModel("attack-fever", AttackMoveType.Rush) },
                }
                );
            StartAttack("attack-fever");
        }
    }
}
