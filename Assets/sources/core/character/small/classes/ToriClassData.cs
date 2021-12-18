using PataRoad.Core.Character.Equipments.Weapons;

namespace PataRoad.Core.Character.Class
{
    internal class ToriClassData : ClassData
    {
        /// <summary>
        /// Determines if it needs to fly high.
        /// </summary>
        private bool _isFever;

        internal ToriClassData(SmallCharacter character) : base(character)
        {
            RootName = "Root/";
            IsFlyingUnit = true;
        }
        protected override void InitLateForClass(Stat realStat)
        {
            if (!(_character is Patapons.Patapon)) FlyUp();
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
        public override void OnAction(Rhythm.Command.RhythmCommandModel model)
        {
            var isFever = model.ComboType == Rhythm.Command.ComboStatus.Fever;
            if (!_isFever && isFever)
            {
                FlyUp();
            }
            else if (_isFever && !isFever)
            {
                FlyDown();
            }
        }
        public override void OnCanceled()
        {
            if (_isFever)
            {
                FlyDown();
            }
        }
        public void FlyUp()
        {
            _animator.AnimateFrom("tori-fly-up");
            _isFever = true;
        }
        private void FlyDown()
        {
            _animator.AnimateFrom("tori-fly-down");
            _isFever = false;
        }
    }
}
