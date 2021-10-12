using Core.Character.Patapon;

namespace Core.Character.Equipment
{
    /// <summary>
    /// Type of equipments. This is combination of <see cref="ClassType"/> and <see cref="EquipmentStyle"/>.
    /// <remarks>
    /// The weapon info has two parts:  in binary [00: Helm, 01: Weapon, 10: Protector(Equipment)]/[0-1000(class)].
    /// For example, Yaripon weapon is 010000 (01/0000), Tatepon shield is 100001 (10/0001), Mahopon shoes are 101000 (10/1000).
    /// If it's helm, it ignores what kind of class is.
    /// </remarks>
    /// <note>The reason to use this logic is for minigame with more predictable weapon.</note>
    /// </summary>
    public enum EquipmentType
    {
        Helm = 0,
        Spear = EquipmentStyle.Weapon | ClassType.Yaripon,
        Sword = EquipmentStyle.Weapon | ClassType.Tatepon,
        Shield = EquipmentStyle.Protector | ClassType.Tatepon,
        Bow = EquipmentStyle.Weapon | ClassType.Yumipon,
        Lance = EquipmentStyle.Weapon | ClassType.Kibapon,
        Horse = EquipmentStyle.Protector | ClassType.Kibapon,
        Javelin = EquipmentStyle.Weapon | ClassType.Toripon,
        Bird = EquipmentStyle.Protector | ClassType.Toripon,
        Club = EquipmentStyle.Weapon | ClassType.Dekapon,
        Shoulder = EquipmentStyle.Protector | ClassType.Dekapon,
        Arm = EquipmentStyle.Weapon | ClassType.Robopon,
        Horn = EquipmentStyle.Weapon | ClassType.Megapon,
        Cape = EquipmentStyle.Protector | ClassType.Megapon,
        Staff = EquipmentStyle.Weapon | ClassType.Mahopon,
        Shoes = EquipmentStyle.Protector | ClassType.Mahopon
    }
    /// <summary>
    /// Weapon adds damage (spear, sword...), and Protector adds defense (shield, shoulder...). Helm is differnt type, and Rarepon can't hold helm.
    /// </summary>
    [System.Flags]
    public enum EquipmentStyle
    {
        Helm = 0,
        Weapon = 16,
        Protector = 32
    }
}
