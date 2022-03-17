﻿using PataRoad.Core.Character.Equipments.Weapons;

namespace PataRoad.Core.Character.Class
{
    internal class MahoClassData : ClassData
    {
        internal MahoClassData(SmallCharacter character) : base(character)
        {
            AdditionalSight = 7;
        }
        protected override void InitLateForClass(Stat realStat)
        {
            AddDefaultModelsToAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<AttackCommandType, AttackMoveModel>()
                {
                    { AttackCommandType.ChargeAttack, GetAttackMoveModel("attack-charge") },
                }
                );
            switch (_attackType)
            {
                case 1:
                    realStat.FireRate += 0.35f;
                    _character.ElementalAttackType = ElementalAttackType.Fire;
                    break;
                case 2:
                    realStat.IceRate += 0.3f;
                    _character.ElementalAttackType = ElementalAttackType.Ice;
                    break;
                case 3:
                    realStat.FireRate += 0.15f;
                    _character.ElementalAttackType = ElementalAttackType.Thunder;
                    break;
            }
        }

        public override void Attack()
        {
            if (!_character.Charged)
            {
                base.Attack();
            }
            else
            {
                _attackController.StartAttack(AttackCommandType.ChargeAttack);
            }
        }
    }
}
