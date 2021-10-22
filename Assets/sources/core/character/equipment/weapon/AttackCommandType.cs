namespace Core.Character.Equipment.Weapon
{
    /// <summary>
    /// Defines what kind of weapon attack should be, depends on the command.
    /// <remarks>This is especially useful for special attack, e.g. Megapon, who has charge defense.</remarks>
    /// <note>Code of Conduct: If ChargeAttack and FeverAttack is same, use FeverAttack. Same for defend, FeverDefend comes first.</note>
    /// </summary>
    public enum AttackCommandType
    {
        Attack,
        ChargeAttack,
        FeverAttack,
        Defend,
        FeverDefend,
        ChargeDefend,
        Charge
    }
}
