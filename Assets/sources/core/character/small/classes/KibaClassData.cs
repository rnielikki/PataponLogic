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
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-fever", GetAttackMoveModel("attack-fever", AttackMoveType.Rush, 2) },
                    { "defend-fever", GetAttackMoveModel("defend-fever", AttackMoveType.Defend).SetAlwaysAnimate() },
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
            if (!_character.OnFever && !_character.Charged)
            {
                _attackController.StartAttack("defend");
            }
            else
            {
                _attackController.StartAttack("defend-fever");
            }
        }
    }
}
