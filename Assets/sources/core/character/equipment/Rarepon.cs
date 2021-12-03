﻿using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    public class Rarepon : Equipment
    {
        public bool IsNormal => CurrentData == null || CurrentData.Index == 0;

        protected override EquipmentType _type => EquipmentType.Rarepon;

        [SerializeField]
        [Tooltip("It can be eye color, and Megapon mouth color etc.")]
        private SpriteRenderer[] _spritesToChangeColor;
        [SerializeField]
        private SpriteRenderer _spriteToHideOnRarepon;
        private void Start()
        {
            Load();
        }
        internal override void ReplaceEqupiment(EquipmentData equipmentData, Stat stat)
        {
            if (HolderData == null) HolderData = GetComponentInParent<SmallCharacterData>();
            var helm = HolderData.EquipmentManager.Helm;

            if (equipmentData.Index != 0)
            {
                helm?.HideEqupiment(stat);
                if (_spriteToHideOnRarepon != null) _spriteToHideOnRarepon.enabled = false;
            }
            else
            {
                if (_spriteToHideOnRarepon != null) _spriteToHideOnRarepon.enabled = true;
                helm?.ShowEqupiment();
            }
            base.ReplaceEqupiment(equipmentData, stat);
        }

        protected override void ReplaceImage(EquipmentData equipmentData)
        {
            base.ReplaceImage(equipmentData);
            var color = (equipmentData as RareponData).Color;

            Changecolor(color);
        }

        private void Changecolor(Color color)
        {
            _spriteRenderers[0].color = color;
            foreach (var renderer in _spritesToChangeColor)
            {
                renderer.color = color;
            }
        }
    }
}
