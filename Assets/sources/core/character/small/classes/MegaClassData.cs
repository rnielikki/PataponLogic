namespace PataRoad.Core.Character.Class
{
    internal class MegaClassData : ClassData
    {
        internal MegaClassData(SmallCharacter character) : base(character)
        {
        }
        protected override void InitLateForClass()
        {
            AddDefaultModelsToAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-fever", GetAttackMoveModel("attack-fever") },
                    { "defend-charge", GetAttackMoveModel("defend-charge", AttackMoveType.Defend, attackDistance: 5) },
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
                _attackController.StartAttack("attack-fever");
            }
        }
        public override void Defend()
        {
            _attackController.StartAttack(_character.Charged ? "defend-charge" : "defend");
        }
    }
}
