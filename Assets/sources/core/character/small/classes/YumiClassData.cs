namespace PataRoad.Core.Character.Class
{
    internal class YumiClassData : ClassData
    {
        private bool _noAttack;
        internal YumiClassData(SmallCharacter character) : base(character)
        {
            ChargeWithoutMove = true;
            AdditionalSight = 10;

        }
        protected override void InitLateForClass(Stat realStat)
        {
            _noAttack = _attackType == 1 && (_character is Patapons.Patapon patapon) && patapon.IsGeneral;
            if (!_noAttack)
            {
                AddDefaultModelsToAttackMoveController()
                    .AddModels(
                    new System.Collections.Generic.Dictionary<Equipments.Weapons.AttackCommandType, AttackMoveModel>()
                    {
                        { Equipments.Weapons.AttackCommandType.FeverAttack, GetAttackMoveModel("attack", attackSpeedMultiplier: 3) },
                    }
                    );
            }
            else
            {
                if (_attackController == null) SetAttackMoveController();
                _attackController.AddModels(
                    new System.Collections.Generic.Dictionary<Equipments.Weapons.AttackCommandType, AttackMoveModel>()
                    {
                        { Equipments.Weapons.AttackCommandType.Defend, GetAttackMoveModel("defend", AttackMoveType.Defend) },
                    }
                    );
            }
        }

        public override void Attack()
        {
            if (_noAttack)
            {
                base.Defend();
            }
            else if (!_character.OnFever && !_character.Charged)
            {
                base.Attack();
            }
            else
            {
                _attackController.StartAttack(Equipments.Weapons.AttackCommandType.FeverAttack);
            }
        }
    }
}
