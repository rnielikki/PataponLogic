using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    [CreateAssetMenu(fileName = "equipment-data-as-number", menuName = "EquipmentData/BowData")]
    class BowData : EquipmentData
    {
        [SerializeField]
        private Sprite _arrowImage;
        public Sprite ArrowImage => _arrowImage;
    }
}
