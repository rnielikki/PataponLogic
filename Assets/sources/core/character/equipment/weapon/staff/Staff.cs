namespace PataRoad.Core.Character.Equipment.Weapon
{
    /// <summary>
    /// Abstraction of some kind of staff behaviours.
    /// </summary>
    public abstract class Staff : WeaponObject
    {
        public abstract void NormalAttack();
        public abstract void ChargeAttack();
        public abstract void Defend();
        public override float MinAttackDistance { get; } = 22;
        public override float WindAttackDistanceOffset { get; } = 4;
        public override void Attack(AttackCommandType attackCommandType)
        {
            switch (attackCommandType)
            {
                case AttackCommandType.Attack:
                    NormalAttack();
                    break;
                case AttackCommandType.ChargeAttack:
                    ChargeAttack();
                    break;
                case AttackCommandType.Defend:
                    Defend();
                    break;
            }
        }
    }
}
