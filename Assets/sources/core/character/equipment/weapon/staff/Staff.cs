namespace Core.Character.Equipment.Weapon
{
    /// <summary>
    /// Abstraction of some kind of staff behaviours.
    /// </summary>
    public abstract class Staff : WeaponObject
    {
        public abstract void NormalAttack();
        public abstract void ChargeAttack();
        public abstract void Defend();
        public override void Attack(AttackType type)
        {
            switch (type)
            {
                case AttackType.Attack:
                    NormalAttack();
                    break;
                case AttackType.ChargeAttack:
                    ChargeAttack();
                    break;
                case AttackType.Defend:
                    Defend();
                    break;
            }
        }
    }
}
