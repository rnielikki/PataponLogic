using PataRoad.Core.Character.Equipments;
using PataRoad.Core.Items;
using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Character.Patapons.Data
{
    /// <summary>
    /// Represents equipment of 'A' Patapon.
    /// </summary>
    [System.Serializable]
    public class PataponEquipmentInfo : ISerializationCallbackReceiver
    {
        private Dictionary<EquipmentType, EquipmentData> _map;

        [SerializeField]
        private string _weaponType;
        [SerializeField]
        private string _protectorType;

        [SerializeField]
        private int _weaponIndex = -1;
        [SerializeField]
        private int _protectorIndex = -1;
        [SerializeField]
        private int _helmIndex = -1;
        [SerializeField]
        private int _rareponIndex = -1;
        [SerializeField]
        private int _gemIndex = -1;
        [SerializeField]
        private bool _isGeneral;

        public PataponEquipmentInfo(Class.ClassType classType, bool isGeneral)
        {
            _map = new Dictionary<EquipmentType, EquipmentData>();
            var equipmentNames = Class.ClassAttackEquipmentData.GetWeaponAndProtectorName(classType);
            _weaponType = equipmentNames.weapon;
            _protectorType = equipmentNames.protector;
            _isGeneral = isGeneral;
        }
        public void Equip(EquipmentData data)
        {
            if (_map.ContainsKey(data.Type))
            {
                _map[data.Type] = data;
            }
            else
            {
                _map.Add(data.Type, data);
            }
        }
        public IEnumerable<EquipmentData> GetAllEquipments() => _map.Values;
        public bool HasEquipment(EquipmentData data) => _map.ContainsKey(data.Type) && _map[data.Type] == data;
        public int GetEquipmentLevel(EquipmentType equipmentType) => _map.ContainsKey(equipmentType) ? _map[equipmentType].LevelGroup : -1;

        private void Serialize()
        {
            _weaponIndex = GetIndex(EquipmentType.Weapon);
            _protectorIndex = GetIndex(EquipmentType.Protector);
            _rareponIndex = GetIndex(EquipmentType.Rarepon);

            if (_rareponIndex < 1) _helmIndex = GetIndex(EquipmentType.Helm);

            int GetIndex(EquipmentType type)
            {
                if (_map.ContainsKey(type))
                {
                    return _map[type].Index;
                }
                else return -1;
            }
        }
        private void Deserialize()
        {
            _map = new Dictionary<EquipmentType, EquipmentData>();
            if (_weaponType != null && _weaponIndex >= 0)
            {
                _map.Add(EquipmentType.Weapon, LoadEquipment(_weaponType, _weaponIndex));
            }
            if (_protectorType != null && _protectorIndex >= 0)
            {
                _map.Add(EquipmentType.Protector, LoadEquipment(_protectorType, _protectorIndex));
            }
            if (!_isGeneral && _rareponIndex >= 0)
            {
                _map.Add(EquipmentType.Rarepon, RareponInfo.LoadResourceWithoutOpen(_rareponIndex));
            }
            if (_rareponIndex <= 0 && _helmIndex >= 0)
            {
                _map.Add(EquipmentType.Helm, LoadEquipment("Helm", _helmIndex));
            }
            if (!_isGeneral && _gemIndex >= 0)
            {
                _map.Add(EquipmentType.Gem, LoadEquipment("Gem", _gemIndex));
            }
        }
        private EquipmentData LoadEquipment(string group, int index) =>
            ItemLoader.GetItem<EquipmentData>(ItemType.Equipment, group, index);

        public void OnBeforeSerialize()
        {
            Serialize();
        }

        public void OnAfterDeserialize()
        {
            Deserialize();
        }
    }
}
