using PataRoad.Core.Character.Equipments.Weapons;

namespace PataRoad.Core.Character.Class
{
    internal class DekaClassData : ClassData
    {
        private bool _isPatapon;
        internal DekaClassData(SmallCharacter character) : base(character)
        {
            IsMeleeUnit = true;
            _isPatapon = character is Patapons.Patapon;
        }
        protected override void InitLateForClass(Stat realStat)
        {
            AddDefaultModelsToAttackMoveController();
            if (_isPatapon)
            {
                _attackController.AddModels(
                    new System.Collections.Generic.Dictionary<AttackCommandType, AttackMoveModel>()
                    {
                        { AttackCommandType.ChargeAttack, GetAttackMoveModel("attack-charge", AttackMoveType.Rush, movingSpeed: 1.5f) },
                    }
                    );
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
                if (_isPatapon)
                {
                    _attackController.StartAttack(AttackCommandType.ChargeAttack);
                }
                else
                {
                    _animator.Animate("attack-charge");
                    _character.DistanceManager.MoveZero(1.6f);
                }
            }
        }
    }
}
