namespace PataRoad.Core.Character.Patapon
{
    /// <summary>
    /// Represents Rarepon info.
    /// </summary>
    public class Rarepon
    {
        /// <summary>
        /// Name of the Rarepon - like Moriussoo, Normal, Wanda...
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// The stat that will give additionally to the Patapon.
        /// </summary>
        public Stat Stat { get; }
        /// <summary>
        /// Sprite image of the Rarepon.
        /// </summary>
        public UnityEngine.Sprite Image { get; }
        /// <summary>
        /// Eye color of the Rarepon.
        /// </summary>
        public UnityEngine.Color Color { get; }
    }
}
