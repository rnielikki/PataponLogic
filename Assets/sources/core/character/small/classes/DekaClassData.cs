namespace PataRoad.Core.Character.Class
{
    internal class DekaClassData : ClassData
    {
        internal DekaClassData(SmallCharacter character) : base(character)
        {
            IsMeleeUnit = true;
        }
        protected override void InitLateForClass(Stat realStat)
        {
            AddDefaultModelsToAttackMoveController();
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
                _character.DistanceManager.MoveZero(1.6f);
                _character.StatusEffectManager.TumbleAttack();
            }
        }
    }
}
