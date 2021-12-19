using PataRoad.Core.Character.Equipments;
using System.Collections.Generic;

namespace PataRoad.Core.Character.Class
{
    /// <summary>
    /// Class data that also needs for euqipment and others. Must be only readable.
    /// </summary>
    public class ClassMetaData
    {
        public string WeaponName { get; }
        public string ProtectorName { get; }

        public string[] AvailableAttackTypes { get; }

        private readonly Dictionary<EquipmentType, string> _nameByEquipmentType = new Dictionary<EquipmentType, string>()
        {
            { EquipmentType.Helm, "Helm" },
            { EquipmentType.Rarepon, "Rarepon" },
        };

        private readonly static Dictionary<ClassType, ClassMetaData> _map = new Dictionary<ClassType, ClassMetaData>()
        {
            { ClassType.Tatepon, new ClassMetaData("Sword","Shield", new string[]{"Attack","Defend only"})},
            { ClassType.Dekapon, new ClassMetaData("Club","Shoulders", new string[]{"Crush"})},
            { ClassType.Robopon, new ClassMetaData("Arms",null, new string[]{"Melee","Throw"})},
            { ClassType.Kibapon, new ClassMetaData("Lance","Horse", new string[]{"Stab"})},
            { ClassType.Yaripon, new ClassMetaData("Spear",null,  new string[]{"Stab"})},
            { ClassType.Megapon, new ClassMetaData("Horn","Cape", new string[]{"Fire on Fever", "Ice on Fever", "No Fever Attk"})},
            { ClassType.Toripon, new ClassMetaData("Javelin","Bird", new string[]{"Stab"})},
            { ClassType.Yumipon, new ClassMetaData("Bow", null, new string[]{"Stab"})},
            { ClassType.Mahopon, new ClassMetaData("Staff","Shoes",  new string[]{"None", "Fire", "Ice", "Thunder"})},
        };

        public static ClassMetaData Get(ClassType type) => _map[type];
        public static string GetEquipmentName(ClassType type, EquipmentType equipmentType) => _map[type].GetEquipmentName(equipmentType);
        private ClassMetaData(string weaponName, string protectorName, string[] availableAttackTypes)
        {
            WeaponName = weaponName;
            ProtectorName = protectorName;
            AvailableAttackTypes = availableAttackTypes;

            _nameByEquipmentType.Add(EquipmentType.Weapon, weaponName);
            _nameByEquipmentType.Add(EquipmentType.Protector, protectorName);
            _nameByEquipmentType.Add(EquipmentType.Gem, "Gem");
        }
        public string GetEquipmentName(EquipmentType type) => _nameByEquipmentType[type];
        public static (string weapon, string protector) GetWeaponAndProtectorName(ClassType type) => (_map[type].WeaponName, _map[type].ProtectorName);
    }
}
