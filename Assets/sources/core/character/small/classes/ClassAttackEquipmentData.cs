using PataRoad.Core.Character.Equipments;
using System.Collections.Generic;

namespace PataRoad.Core.Character.Class
{
    /// <summary>
    /// Class data that also needs for euqipment and others. Must be only readable.
    /// </summary>
    /// <note>This must work without <see cref="SmallCharacter"/>, for Equipment scene. DO NOT MERGE To <see cref="ClassData"/>.</note>
    public class ClassAttackEquipmentData
    {
        public string WeaponName { get; }
        public string ProtectorName { get; }

        public string[] AvailableAttackTypes { get; }

        private readonly Dictionary<EquipmentType, string> _nameByEquipmentType = new Dictionary<EquipmentType, string>()
        {
            { EquipmentType.Helm, "Helm" },
            { EquipmentType.Rarepon, "Rarepon" },
        };

        private readonly static Dictionary<ClassType, ClassAttackEquipmentData> _map = new Dictionary<ClassType, ClassAttackEquipmentData>()
        {
            { ClassType.Tatepon, new ClassAttackEquipmentData("Sword","Shield", new string[]{"Attack","Defend only"})},
            { ClassType.Dekapon, new ClassAttackEquipmentData("Club","Shoulders", new string[]{"Crush"})},
            { ClassType.Robopon, new ClassAttackEquipmentData("Arms",null, new string[]{"Melee","Throw"})},
            { ClassType.Kibapon, new ClassAttackEquipmentData("Lance","Horse", new string[]{"Stab"})},
            { ClassType.Yaripon, new ClassAttackEquipmentData("Spear",null,  new string[]{"Stab"})},
            {
                ClassType.Megapon,
                new ClassAttackEquipmentData("Horn","Cape", new string[]{"Fire on Fever", "Ice on Fever", "No Fever Attk"})},
            { ClassType.Toripon, new ClassAttackEquipmentData("Javelin","Bird", new string[]{"Stab"})},
            { ClassType.Yumipon, new ClassAttackEquipmentData("Bow", null, new string[]{"Stab", "No Cannon Attk"})},
            { ClassType.Mahopon, new ClassAttackEquipmentData("Staff","Shoes",  new string[]{"None", "Fire", "Ice", "Thunder"})},
        };

        public static ClassAttackEquipmentData Get(ClassType type) => _map[type];
        public static string GetEquipmentName(ClassType type, EquipmentType equipmentType) => _map[type].GetEquipmentName(equipmentType);
        private ClassAttackEquipmentData(string weaponName, string protectorName, string[] availableAttackTypes)
        {
            WeaponName = weaponName;
            ProtectorName = protectorName;
            AvailableAttackTypes = availableAttackTypes;

            _nameByEquipmentType.Add(EquipmentType.Weapon, weaponName);
            _nameByEquipmentType.Add(EquipmentType.Protector, protectorName);
            _nameByEquipmentType.Add(EquipmentType.Gem, "Gem");
        }
        public string GetEquipmentName(EquipmentType type) => _nameByEquipmentType[type];
        public static string GetRandomEquipmentName(ClassType[] types)
        {
            var randomClassIndex = UnityEngine.Random.Range(0, types.Length);
            var classType = types[randomClassIndex];
            var data = _map[classType];
            if (data.ProtectorName == null) return data.WeaponName;
            else
            {
                return (UnityEngine.Random.Range(0, 2) == 0) ? data.WeaponName : data.ProtectorName;
            }
        }
        public static (string weapon, string protector) GetWeaponAndProtectorName(ClassType type)
            => (_map[type].WeaponName, _map[type].ProtectorName);
    }
}
