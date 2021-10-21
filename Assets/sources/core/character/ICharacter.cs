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
        /// <summary>
        /// Current Hit point.
        /// <remarks>It shouldn't be bigger than <see cref="Stat.HitPoint"/> or smaller than 0. If this value is 0, it causes death.</remarks>
        /// </summary>
        public int CurrentHitPoint { get; }

        /// <summary>
        /// Child will call this method when collision is detected.
        /// </summary>
        /// <param name="other">The collision parameter from <see cref="UnityEngine.OnCollisionEnter2D"/></param>
        public void TakeCollision(UnityEngine.Collision2D other);

        public void TakeDamage(int value);
    }
}
