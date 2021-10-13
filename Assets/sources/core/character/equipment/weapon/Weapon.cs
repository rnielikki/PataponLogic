using UnityEngine;

namespace Core.Character.Equipment.Weapon
{
    /// <summary>
    /// Represents any weapon DATA. For game object, see <see cref="WeaponObject"/>.
    /// </summary>
    public class Weapon : IEquipment
    {
        public string Name { get; set; }

        public Stat Stat { get; set; }

        public Sprite Image { get; set; }
    }
}
