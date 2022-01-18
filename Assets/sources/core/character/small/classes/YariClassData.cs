using PataRoad.Core.Character.Equipments.Weapons;

namespace PataRoad.Core.Character.Class
{
    internal class YariClassData : ClassData
    {
        internal YariClassData(SmallCharacter character) : base(character)
        {
            AdditionalSight = 3;
        }
        protected override void InitLateForClass(Stat realStat)
        {
            AddDefaultModelsToAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<AttackCommandType, AttackMoveModel>()
                {
                    { AttackCommandType.FeverAttack, GetAttackMoveModel("attack-fever") },
                }
                );
        }

        public override void Attack()
        {
            if (!_character.OnFever && !_character.Charged)
            {
                base.Attack();
            }
            else
            {
                _attackController.StartAttack(AttackCommandType.FeverAttack);
            }
        }
    }
}
