using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.Core.Character.Patapons.Data
{
    /// <summary>
    /// Represents equipment of 'A' Patapon.
    /// </summary>
    [System.Serializable]
    public class PataponEquipmentInfo
    {
        private EquipmentData _weaponData;
        private EquipmentData _protectorData;
        private EquipmentData _helmData;
        private EquipmentData _rareponData;
        public EquipmentData WeaponData => _weaponData;
        public EquipmentData ProtectorData => _protectorData;
        public EquipmentData HelmData => _helmData;
        public EquipmentData RareponData => _rareponData;
        [SerializeField]
        private string _weaponType;
        [SerializeField]
        private int _weaponIndex;
        [SerializeField]
        private string _protectorType;
        [SerializeField]
        private int _protectorIndex;
        [SerializeField]
        private int _helmIndex;
        [SerializeField]
        private int _rareponIndex;

        public PataponEquipmentInfo(Class.ClassType classType)
        {
            var equipmentNames = SmallCharacterData.GetWeaponAndProtectorName(classType);
            _weaponType = equipmentNames.weapon;
            _protectorType = equipmentNames.protector;
        }
        public void Serialize()
        {
            _weaponIndex = _weaponData.Index;
            _protectorIndex = _protectorData.Index;
            _rareponIndex = _rareponData.Index;
            if (_rareponIndex == 0) _helmIndex = _helmData.Index;
        }
        public void Deserialize()
        {
            _weaponData = LoadEquipment(_weaponType, _weaponIndex);
            _protectorData = LoadEquipment(_protectorType, _protectorIndex);
            _rareponData = LoadEquipment("Rarepon", _rareponIndex);
            if (_rareponIndex == 0) _helmData = LoadEquipment("Helm", _helmIndex);
        }
        private EquipmentData LoadEquipment(string group, int index) =>
            ItemLoader.GetItem<EquipmentData>(ItemType.Equipment, group, index);
    }
}
