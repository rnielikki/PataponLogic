namespace PataRoad.Core.Character.Class
{
    internal class YumiClassData : ClassData
    {
        private bool _noAttack;
        private bool _isGeneral;
        internal YumiClassData(SmallCharacter character) : base(character)
        {
            AdditionalSight = 10;
        }
        protected override void InitLateForClass(Stat realStat)
        {
            _isGeneral = (_character is Patapons.Patapon patapon) && patapon.IsGeneral;
            _noAttack = _attackType == 1 && _isGeneral;
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
            }
        }

        public override void Attack()
        {
            if (_noAttack)
            {
                _animator.Animate("defend");
                _character.DistanceManager.MoveToInitialPlace(_character.Stat.MovementSpeed);
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
        public override void Defend()
        {
            if (!_isGeneral) base.Defend();
            else
            {
                _animator.Animate("defend");
                _character.DistanceManager.MoveToInitialPlace(_character.Stat.MovementSpeed);
            }
        }
    }
}
