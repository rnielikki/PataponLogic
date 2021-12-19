using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    [CreateAssetMenu(fileName = "equipment-data-as-number", menuName = "Item/EquipmentData/GemData")]
    class GemData : EquipmentData
    {
        [SerializeField]
        ElementalAttackType _elementalAttackType;
        public ElementalAttackType ElementalAttackType => _elementalAttackType;
        [SerializeField]
        Color _weaponColor;
        public Color WeaponColor => _weaponColor;
    }
}

