namespace PataRoad.Core.Character.Patapons.General
{
    /// <summary>
    /// Represents general effect while general IS ALIVE. This is different from general mode.
    /// </summary>
    public interface IGeneralEffect
    {
        /// <summary>
        /// Adds additional effect to self.
        /// </summary>
        public void SelfEffect(Patapon patapon);
        /// <summary>
        /// The effect that supports the general's group, while general is alive.
        /// </summary>
        public void StartGroupEffect(System.Collections.Generic.IEnumerable<Patapon> patapons);
        /// <summary>
        /// Remove group effect when the general dies.
        /// </summary>
        public void EndGroupEffect(System.Collections.Generic.IEnumerable<Patapon> patapons);
    }
}
