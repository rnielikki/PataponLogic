using UnityEngine;

namespace PataRoad.Core.Character.Patapons
{
    class Yumipon : Patapon
    {
        private void Awake()
        {
            Init();
            Class = ClassType.Yumipon;
        }
        private void Start()
        {
            AddDefaultModelsToAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-fever", GetAttackMoveModel("attack", attackSpeedMultiplier: 3) },
                }
                );

            WeaponLoadTest("Weapons/Bow/1");
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
        protected override void Charge() => ChargeWithoutMoving();
    }
}
