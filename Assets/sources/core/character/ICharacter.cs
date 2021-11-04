namespace Core.Character
{
    /// <summary>
    /// Represents any basic charcter logic. It can be Patapon, enemy or boss. DOESN'T represent structures, though.
    /// </summary>
    public interface ICharacter : IAttackable
    {
        /// <summary>
        /// Get current damage that can inflict - for Patapon, "how perfect the drums are" affects.
        /// </summary>
        public int GetAttackDamage();
        public Equipment.Weapon.AttackType AttackType { get; }
        public void OnAttackHit(UnityEngine.Vector2 point);
        public void OnAttackMiss(UnityEngine.Vector2 point);
    }
}
