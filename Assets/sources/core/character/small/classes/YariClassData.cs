namespace PataRoad.Core.Character.Class
{
    internal class YariClassData : ClassData
    {
        internal YariClassData(SmallCharacter character) : base(character)
        {
        }
        protected override void InitLateForClass(Stat realStat)
        {
            AddDefaultModelsToAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-fever", GetAttackMoveModel("attack-fever") },
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
    }
}
