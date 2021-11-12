using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    [CreateAssetMenu(fileName = "equipment-data-as-number", menuName = "EquipmentData/StaffData")]
    class StaffData : EquipmentData
    {
        [SerializeField]
        private GameObject[] _additionalPrefabs;
        public GameObject[] AdditionalPrefabs => _additionalPrefabs;
    }
}
