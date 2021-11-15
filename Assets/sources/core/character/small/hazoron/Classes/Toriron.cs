using PataRoad.Core.Character.Patapons;

namespace PataRoad.Core.Character.Hazorons
{
    public class Toriron : Hazoron
    {
        private void Awake()
        {
            RootName = "Root/";
            IsFlyingUnit = true;
            Init();
            CharacterSize = transform.Find("Root/Patapon-body/Face").GetComponent<UnityEngine.CircleCollider2D>().radius + AttackDistance;
            Class = ClassType.Toripon;
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
            CharAnimator.AnimateFrom("tori-fly-up");
            StartAttack("attack-fever");
        }
        protected override void BeforeDie()
        {
            CharAnimator.AnimateFrom("tori-fly-stop");
        }
    }
}
