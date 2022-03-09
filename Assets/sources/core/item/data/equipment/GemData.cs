using PataRoad.Core.Character.Equipments.Weapons;
using UnityEngine;

namespace PataRoad.Core.Items
{
    [CreateAssetMenu(fileName = "equipment-data-as-number", menuName = "Item/EquipmentData/GemData")]
    class GemData : EquipmentData
    {
        [SerializeField]
        ElementalAttackType _elementalAttackType;
        public ElementalAttackType ElementalAttackType => _elementalAttackType;
        [SerializeField]
        Material _weaponMaterial;
        public Material WeaponMaterial => _weaponMaterial;
    }
}
