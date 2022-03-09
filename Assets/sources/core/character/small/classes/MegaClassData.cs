using PataRoad.Core.Character.Equipments.Weapons;
using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Class
{
    internal class MegaClassData : ClassData
    {
        internal MegaClassData(SmallCharacter character) : base(character)
        {
            AdditionalSight = 3;
        }
        protected override void InitLateForClass(Stat realStat)
        {
            switch (_attackType)
            {
                case 0:
                case 1:
                    AddDefaultModelsToAttackMoveController()
                        .AddModels(
                    new System.Collections.Generic.Dictionary<AttackCommandType, AttackMoveModel>()
                    {
                        { AttackCommandType.FeverAttack, GetAttackMoveModel("attack-fever") },
                        { AttackCommandType.ChargeDefend, GetAttackMoveModel("defend-charge", AttackMoveType.Defend) },
                    }
                    );
                    break;
                case 2:
                    AddDefaultModelsToAttackMoveController()
                        .AddModels(
                    new System.Collections.Generic.Dictionary<AttackCommandType, AttackMoveModel>()
                    {
                        { AttackCommandType.ChargeDefend, GetAttackMoveModel("defend-charge", AttackMoveType.Defend) },
                    }
                    );
                    break;
            }
        }

        public override void Attack()
        {
            if ((!_character.OnFever && !_character.Charged) || _attackType == 2)
            {
                base.Attack();
            }
            else
            {
                _attackController.StartAttack(AttackCommandType.FeverAttack);
            }
        }
        public override void PerformCommandAction(CommandSong song)
        {
            if (song == CommandSong.Ponchaka) _animator.Animate("charge");
        }
        public override void Defend()
        {
            _attackController.StartAttack(_character.Charged ? AttackCommandType.ChargeDefend : AttackCommandType.Defend);
        }
    }
}
