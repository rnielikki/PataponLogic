namespace PataRoad.Core.Character
{
    /// <summary>
    /// Represents any attackable object, including struct and any character.
    /// </summary
    public interface IAttackable
    {
        /// <summary>
        /// Stat data. Even if the object is not attacking, this is necessary for defense.
        /// </summary>
        public Stat Stat { get; }

        /// <summary>
        /// Current Hit point.
        /// <remarks>It shouldn't be bigger than <see cref="Stat.HitPoint"/> or smaller than 0. If this value is 0, it causes death.</remarks>
        /// </summary>
        public int CurrentHitPoint { get; }
        /// <summary>
        /// Calls when health is 0. This should contain destroying action; like destroying animation -> destroy
        /// </summary>
        public void Die();

        /// <summary>
        /// The DIRECT damage the object will get. Expected to be calculated through <see cref="Equipment.Logic.DamageCalculator"/>.
        /// </summary>
        public void TakeDamage(int damage);
    }
}
