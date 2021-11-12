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
        public Protector Protector { get; private set; }
        public Rarepon Rarepon { get; private set; }
        private Dictionary<EquipmentType, Equipment> _map;

        public EquipmentManager(GameObject target)
        {
            Helm = target.GetComponentInChildren<Helm>();
            Weapon = target.GetComponentInChildren<Weapon>();
            Protector = target.GetComponentInChildren<Protector>();
            Rarepon = target.GetComponentInChildren<Rarepon>();
            _map = new Dictionary<EquipmentType, Equipment>()
            {
                { EquipmentType.Helm, Helm},
                { EquipmentType.Weapon, Weapon},
                { EquipmentType.Protector, Protector},
                { EquipmentType.Rarepon, Rarepon}
            };
        }
        public Stat Equip(EquipmentData equipmentData, Stat stat)
        {
            if (_map.TryGetValue(equipmentData.Type, out Equipment eq))
            {
                eq.ReplaceEqupiment(equipmentData, stat);
            }
            return stat;
        }
    }
}
