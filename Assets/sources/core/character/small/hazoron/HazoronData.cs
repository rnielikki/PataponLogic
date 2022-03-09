using PataRoad.Core.Character.Equipments;
using PataRoad.Core.Items;
using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Character
{
    public class HazoronData : SmallCharacterData
    {
        [SerializeField]
        private EquipmentData _weaponData;
        [SerializeField]
        private EquipmentData _protectorData;
        [SerializeField]
        private EquipmentData _gemData;

        [SerializeField]
        private int _attackTypeIndex;
        public int AttackTypeIndex => _attackTypeIndex;
        protected override IEnumerable<EquipmentData> GetEquipmentData() =>
            new EquipmentData[] { _weaponData, _protectorData, _gemData };

        private void OnValidate()
        {
            if (_weaponData != null && _weaponData.Type != EquipmentType.Weapon)
            {
                throw new System.ArgumentException("Weapon data should be type of weapon but it's " + _weaponData.Type);
            }
            if (_protectorData != null && _protectorData.Type != EquipmentType.Protector)
            {
                throw new System.ArgumentException("Protector should be type of protector but it's " + _protectorData.Type);
            }
            if (_gemData != null && _gemData.Type != EquipmentType.Gem)
            {
                throw new System.ArgumentException("Element Gem should be type of gem but it's " + _gemData.Type);
            }
        }
    }
}
