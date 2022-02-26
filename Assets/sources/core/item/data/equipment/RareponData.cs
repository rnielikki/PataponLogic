using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments
{
    [CreateAssetMenu(fileName = "equipment-data-as-number", menuName = "Item/EquipmentData/RareponData")]
    public class RareponData : EquipmentData
    {
        [SerializeReference]
        int _level;
        public int Level => _level;

        [SerializeField]
        private Color _color;
        public Color Color => _color;

        [SerializeReference]
        AttackTypeResistance _attackTypeResistance = new AttackTypeResistance();

        public AttackTypeResistance AttackTypeResistance => _attackTypeResistance;
    }
}
