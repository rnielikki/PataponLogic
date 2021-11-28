namespace PataRoad.Core.Character.Class
{
    internal class MahoClassData : ClassData
    {
        internal MahoClassData(SmallCharacter character) : base(character)
        {
            ChargeWithoutMove = true;
        }
        protected override void InitLateForClass()
        {
            AddDefaultModelsToAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-charge", GetAttackMoveModel("attack-charge") },
                }
                );
        }

        public override void Attack()
        {
            if (!_character.Charged)
            {
                base.Attack();
            }
            else
            {
                _animator.Animate("attack-charge");
            }
        }
    }
}
