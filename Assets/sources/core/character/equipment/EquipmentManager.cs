using PataRoad.Core.Character.Equipments.Weapons;
using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments
{
    public class EquipmentManager
    {
        /// <summary>
        /// Helm that the character is currently using.
        /// <note>This will do nothing unless if <see cref="Rarepon"/> is set to normal.</note>
        /// </summary>
        public Helm Helm { get; private set; }
        /// <summary>
        /// Weapon (e.g. spear, swrod...) that the character uses.
        /// </summary>
        public Weapon Weapon { get; private set; }
        /// <summary>
        /// Protector (e.g. horse, shoes, shield, shoulder...) that Patapon uses.
        /// <note>This is useless for specific classes, like Yaripon or Yumipon.</note>
        /// </summary>
        public Equipment Protector { get; private set; }
        public Rarepon Rarepon { get; private set; }

        private readonly Dictionary<EquipmentType, Equipment> _equipments = new Dictionary<EquipmentType, Equipment>();

        public EquipmentManager(GameObject target)
        {
            foreach (var equipment in target.GetComponentsInChildren<Equipment>())
            {
                if (_equipments.ContainsKey(equipment.Type)) throw new System.ArgumentException("A target cannot contain more than one identical type of equipment. Duplication found: " + equipment.Type + ", from " + equipment.name);
                _equipments.Add(equipment.Type, equipment);
            }
            Helm = (Helm)TryGetEquipment(EquipmentType.Helm);
            Weapon = (Weapon)TryGetEquipment(EquipmentType.Weapon);
            Protector = TryGetEquipment(EquipmentType.Protector);
            Rarepon = (Rarepon)TryGetEquipment(EquipmentType.Rarepon);
        }
        public Stat Equip(EquipmentData equipmentData, Stat stat)
        {
            if (_equipments.TryGetValue(equipmentData.Type, out Equipment eq))
            {
                eq.ReplaceEqupiment(equipmentData, stat);
            }
            return stat;
        }
        private Equipment TryGetEquipment(EquipmentType type)
        {
            if (_equipments.TryGetValue(type, out Equipment equipment)) return equipment;
            else return null;
        }
    }
}
