namespace Core.Character.Equipment.Weapon
{
    /// <summary>
    /// Defines what kind of damage will dealt. This affects damage amount, depends on the target.
    /// </summary>
    public enum AttackType
    {
        Neutral,
        Crush,
        Slash,
        Stab,
        Magic,
        Sound
    }
    /// <summary>
    /// Defines elemental attack type, like fire or ice.
    /// </summary>
    public enum ElementalAttackType
    {
        Fire,
        Ice
    }
}
