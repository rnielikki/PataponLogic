using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    [CreateAssetMenu(fileName = "equipment-data-as-number", menuName = "EquipmentData/RareponData")]
    public class RareponData : EquipmentData
    {
        [SerializeField]
        private Color _color;
        public Color Color => _color;
    }
}
