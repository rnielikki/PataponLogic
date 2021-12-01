using PataRoad.Core.Character.Equipments.Weapons;
using PataRoad.Core.Items;
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
        private readonly Dictionary<EquipmentType, Equipment> _map;
        private readonly SmallCharacterData _target;

        public EquipmentManager(SmallCharacterData target)
        {
            Helm = target.GetComponentInChildren<Helm>();
            Weapon = target.GetComponentInChildren<Weapon>();
            Protector = target.GetComponentInChildren<Protector>();
            Rarepon = target.GetComponentInChildren<Rarepon>();

            _map = new Dictionary<EquipmentType, Equipment>();
            _target = target;
        }
        public void Init()
        {
            AddToMapIfNotNull(EquipmentType.Helm, Helm, "Helm", _target);
            AddToMapIfNotNull(EquipmentType.Weapon, Weapon, _target.WeaponName, _target);
            AddToMapIfNotNull(EquipmentType.Protector, Protector, _target.ProtectorName, _target);
            AddToMapIfNotNull(EquipmentType.Rarepon, Rarepon, "Rarepon", _target);
        }

        private void AddToMapIfNotNull<T>(EquipmentType type, T equipment, string equipmentGroup, SmallCharacterData target) where T : Equipment
        {
            if (equipment != null && !equipment.FixedEquipment)
            {
                _map.Add(type, equipment);
                if (type != EquipmentType.Rarepon) Equip(ItemLoader.GetItem<EquipmentData>(ItemType.Equipment, equipmentGroup, 0), target.Stat);
            }
        }
        public void Equip(EquipmentData equipmentData, Stat stat)
        {
            if (_map.TryGetValue(equipmentData.Type, out Equipment eq))
            {
                eq.ReplaceEqupiment(equipmentData, stat);
            }
        }
    }
}
