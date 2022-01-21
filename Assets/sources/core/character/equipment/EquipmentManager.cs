using PataRoad.Core.Character.Equipments.Weapons;
using PataRoad.Core.Items;
using System.Collections.Generic;

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
        public ElementGem ElementGem { get; private set; }
        private readonly Dictionary<EquipmentType, Equipment> _map;
        public IEnumerable<Equipment> Equipments => _map.Values;
        private readonly SmallCharacterData _target;

        public EquipmentManager(SmallCharacterData target)
        {
            Helm = target.GetComponentInChildren<Helm>();
            Weapon = target.GetComponentInChildren<Weapon>();
            Protector = target.GetComponentInChildren<Protector>();
            Rarepon = target.GetComponentInChildren<Rarepon>();
            ElementGem = target.GetComponentInChildren<ElementGem>();

            _map = new Dictionary<EquipmentType, Equipment>();
            _target = target;
        }
        public void Init(IEnumerable<EquipmentData> equipmentData)
        {
            AddToMapIfNotNull(EquipmentType.Weapon, Weapon);
            AddToMapIfNotNull(EquipmentType.Protector, Protector);
            AddToMapIfNotNull(EquipmentType.Rarepon, Rarepon);
            AddToMapIfNotNull(EquipmentType.Helm, Helm);
            AddToMapIfNotNull(EquipmentType.Gem, ElementGem);


            foreach (var eq in equipmentData)
            {
                Equip(eq);
            }
            foreach (var kv in _map)
            {
                if (kv.Value.CurrentData == null)
                {
                    _target.Equip(
                        ItemLoader.GetItem<EquipmentData>(
                            ItemType.Equipment, Class.ClassAttackEquipmentData.GetEquipmentName(_target.Type, kv.Key), 0
                            ));
                }
            }
        }
        private void AddToMapIfNotNull<T>(EquipmentType type, T equipment) where T : Equipment
        {
            //, string equipmentGroup, SmallCharacterData target
            if (equipment != null && !equipment.FixedEquipment)
            {
                _map.Add(type, equipment);
            }
        }
        public void Equip(EquipmentData equipmentData)
        {
            if (equipmentData == null) return;
            if (_map.TryGetValue(equipmentData.Type, out Equipment eq))
            {
                eq.ReplaceEqupiment(equipmentData, _target.Stat);
            }
        }
        public void EquipDefault(EquipmentType type)
        {
            if (_map.TryGetValue(type, out Equipment eq))
            {
                eq.EquipDefault(_target.Stat);
            }
        }
        public EquipmentData GetEquipmentData(EquipmentType type)
        {
            if (_map.TryGetValue(type, out Equipment eq))
            {
                return eq.CurrentData;
            }
            else return null;
        }
        public IEnumerable<EquipmentType> GetAvailableEquipmentTypes => _map.Keys;
        public bool CanEquipHelm()
        {
            return ((Rarepon == null) || Rarepon.IsNormal) && Helm != null;
        }
    }
}
