namespace PataRoad.Core.Character.Equipment
{
    /// <summary>
    /// Represents any equipment DATA that Patapon (or enemy) use, like spear, shield, cape, helm...
    /// </summary>
    public interface IEquipment
    {
        ///<summary>
        /// Name of the equipment, e.g. "Wooden Shield", "Divine Sword"
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// The stat bonus that attached to the weapon.
        /// </summary>
        public Stat Stat { get; }
        /// <summary>
        /// Sprite image of the weapon.
        /// </summary>
        public UnityEngine.Sprite Image { get; }
    }
}
