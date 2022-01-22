﻿using PataRoad.Core.Items;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    public class ElementGem : Equipment
    {
        protected override EquipmentType _type => EquipmentType.Gem;
        protected override void LoadRenderersAndImage()
        {
            //no renderer or image.
            _spriteRenderers = System.Array.Empty<UnityEngine.SpriteRenderer>();
        }
        internal override void ReplaceEqupiment(EquipmentData equipmentData, Stat stat)
        {
            if (equipmentData is GemData gemData)
            {
                base.ReplaceEqupiment(equipmentData, stat);
                if (HolderData.EquipmentManager?.Weapon != null)
                {
                    HolderData.EquipmentManager.Weapon.Colorize(gemData.WeaponMaterial);
                }
                HolderData.ElementalAttackType = gemData.ElementalAttackType;
            }
        }
        protected override void ReplaceImage(EquipmentData equipmentData)
        {
            //no image to replace.
        }
    }
}

