using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    [CreateAssetMenu(fileName = "equipment-data-as-number", menuName = "Item/EquipmentData/StaffData")]
    class StaffData : EquipmentData
    {
        [SerializeField]
        private GameObject _additionalPrefab;
        public GameObject AdditionalPrefab => _additionalPrefab;
    }
}
