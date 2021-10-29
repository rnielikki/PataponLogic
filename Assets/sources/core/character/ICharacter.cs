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
        /// <param name="other">The collision parameter from <see cref="UnityEngine.OnTriggerEnter2D"/></param>
        public void TakeDamage(UnityEngine.Collider2D other);
        /// <summary>
        /// Get current damage that can inflict - for Patapon, "how perfect the drums are" affects.
        /// </summary>
        public int GetCurrentDamage();
        public Equipment.Weapon.AttackType AttackType { get; }
    }
}
