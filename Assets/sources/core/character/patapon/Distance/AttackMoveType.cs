namespace Core.Character.Patapon
{
    /// <summary>
    /// This determines attack distance for <see cref="AttackMoveController"/>.
    /// </summary>
    /// <remarks>
    /// * Attack: Go close to the target and attack.
    /// * Attack: Go ZERO offset to the whole manager position and attack.
    /// * Rush: Go forward, even IF there's NO TARGET in sight. Kibapon fever attack is good example.
    /// </remarks>
    public enum AttackMoveType
    {
        Attack,
        Defend,
        Rush
    }
}
