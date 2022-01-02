using PataRoad.Core.Character.Equipments.Weapons;

namespace PataRoad.Core.Character.Class
{
    internal class KibaClassData : ClassData
    {
        internal KibaClassData(SmallCharacter character) : base(character)
        {
            IsMeleeUnit = true;
        }
        protected override void InitLateForClass(Stat realStat)
        {
            AddDefaultModelsToAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<AttackCommandType, AttackMoveModel>()
                {
                    { AttackCommandType.FeverAttack, GetAttackMoveModel("attack-fever", AttackMoveType.Rush, 2) },
                    { AttackCommandType.FeverDefend, GetAttackMoveModel("defend-fever", AttackMoveType.Defend).SetAlwaysAnimate() },
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
        public override void Defend()
        {
            if (!_character.OnFever && !_character.Charged)
            {
                _attackController.StartAttack(AttackCommandType.Defend);
            }
            else
            {
                _attackController.StartAttack(AttackCommandType.FeverDefend);
            }
        }
        public override bool IsInAttackDistance()
        {
            var closest = _character.DistanceCalculator.GetClosestForAttack();
            if (closest == null) return false;

            return _character.DistanceCalculator.IsInTargetRange(closest.Value.x, _character.CharacterSize);
        }
    }
}
