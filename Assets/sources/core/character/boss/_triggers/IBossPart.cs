namespace PataRoad.Core.Character.Bosses
{
    /// <summary>
    /// Part of any boss, that has own hit point.
    /// </summary>
    interface IBossPart
    {
        /// <summary>
        /// Takes damage and returns the damage that the whole boss will take finally.
        /// </summary>
        /// <param name="damage">Original (and final) damage from the boss.</param>
        /// <returns>Final damage that will deal to the whole boss.</returns>
        public float TakeDamage(int damage);
    }
}