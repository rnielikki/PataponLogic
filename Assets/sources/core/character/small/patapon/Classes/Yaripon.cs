using UnityEngine;

namespace PataRoad.Core.Character.Patapons
{
    public class Yaripon : Patapon
    {
        private void Awake()
        {
            Init();
            Class = ClassType.Yaripon;
        }
        private void Start()
        {
            AddDefaultModelsToAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-fever", GetAttackMoveModel("attack-fever") },
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
        public override General.IGeneralEffect GetGeneralEffect() => new General.PrincessEffect();
    }
}
