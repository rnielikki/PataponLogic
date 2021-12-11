namespace PataRoad.Core.Character.Class
{
    internal class YumiClassData : ClassData
    {
        internal YumiClassData(SmallCharacter character) : base(character)
        {
            ChargeWithoutMove = true;
        }
        protected override void InitLateForClass(Stat realStat)
        {
            AddDefaultModelsToAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-fever", GetAttackMoveModel("attack", attackSpeedMultiplier: 3) },
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
