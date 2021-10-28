namespace Core.Character
{
    /// <summary>
    /// Represents any basic charcter logic. It can be Patapon, enemy or boss. DOESN'T represent structures, though.
    /// </summary>
    public interface ICharacter : IAttackable
    {
        /// <summary>
        /// Child will call this method when collision is detected.
        /// </summary>
        /// <param name="other">The collision parameter from <see cref="UnityEngine.OnCollisionEnter2D"/></param>
        public void TakeCollision(UnityEngine.Collision2D other);
    }
}
