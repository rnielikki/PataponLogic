namespace PataRoad.Core.Character.Class
{
    internal class MahoClassData : ClassData
    {
        internal MahoClassData(SmallCharacter character) : base(character)
        {
            ChargeWithoutMove = true;
        }
        protected override void InitLateForClass(Stat realStat)
        {
            AddDefaultModelsToAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-charge", GetAttackMoveModel("attack-charge") },
                }
                );
            switch (_attackType)
            {
                case 1:
                    realStat.FireRate += 0.35f;
                    break;
                case 2:
                    realStat.IceRate += 0.25f;
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
                _animator.Animate("attack-charge");
            }
        }
    }
}
