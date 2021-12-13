using PataRoad.Core.Items;
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
        protected override EquipmentData GetDefault() => Global.GlobalData.PataponInfo.RareponInfo.GetRarepon(0);
        internal override void ReplaceEqupiment(EquipmentData equipmentData, Stat stat)
        {
            if (HolderData == null) Load();
            var helm = HolderData.EquipmentManager.Helm;

            if (equipmentData.Index != 0)
            {
                helm?.HideEqupiment(stat);
                if (_spriteToHideOnRarepon != null) _spriteToHideOnRarepon.enabled = false;
            }
            else if (helm != null)
            {
                helm.ShowEqupiment();
            }
            else if (_spriteToHideOnRarepon != null)
            {
                _spriteToHideOnRarepon.enabled = true;
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
