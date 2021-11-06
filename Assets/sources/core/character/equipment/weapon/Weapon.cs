using UnityEngine;

namespace PataRoad.Core.Character.Equipment.Weapon
{
    /// <summary>
    /// Represents any weapon DATA. For game object, see <see cref="WeaponObject"/>.
    /// </summary>
    public class Weapon : IEquipment
    {
        public string Name { get; set; }

        public Stat Stat { get; set; }

        public Sprite Image { get; set; }

        public float Mass { get; set; }
    }
}
