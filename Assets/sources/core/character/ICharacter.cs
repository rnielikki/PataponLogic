namespace Core.Character
{
    /// <summary>
    /// Represents any basic charcter logic. It can be Patapon, enemy or boss. DOESN'T represent structures, though.
    /// </summary>
    public interface ICharacter
    {
        /// <summary>
        /// Stat data. Every character has it, and it should be calculated for e.g. damage dealing or status effects.
        /// </summary>
        public Stat Stat { get; }
    }
}
