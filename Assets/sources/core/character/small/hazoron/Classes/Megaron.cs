using PataRoad.Core.Character.Patapons;

namespace PataRoad.Core.Character.Hazorons
{
    class Megaron : Hazoron
    {
        private void Awake()
        {
            Init();
            Class = ClassType.Megapon;
        }
        void Start()
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
