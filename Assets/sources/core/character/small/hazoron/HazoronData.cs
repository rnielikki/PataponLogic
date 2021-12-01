using PataRoad.Core.Character.Equipments;
using UnityEngine;

namespace PataRoad.Core.Character
{
    public class HazoronData : SmallCharacterData
    {
        [SerializeField]
        private Items.EquipmentData _weaponData;
        [SerializeField]
        private Items.EquipmentData _protectorData;

        protected override void SetEquipments()
        {
            WeaponData = _weaponData;
            ProtectorData = _protectorData;
        }
        private void OnValidate()
        {
            if (_weaponData != null && _weaponData.Type != EquipmentType.Weapon)
            {
                throw new System.ArgumentException("Weapon data should be type of weapon but it's " + WeaponData.Type);
            }
            if (_protectorData != null && _protectorData.Type != EquipmentType.Protector)
            {
                throw new System.ArgumentException("Protector should be type of protector but it's " + ProtectorData.Type);
            }
        }

    }
}
