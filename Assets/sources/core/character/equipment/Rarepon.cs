using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments
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
        protected override EquipmentData GetDefault() => Global.GlobalData.CurrentSlot.PataponInfo.RareponInfo.DefaultRarepon.Data;
        internal override void ReplaceEqupiment(EquipmentData equipmentData, Stat stat)
        {
            if (HolderData == null) Load();
            var helm = HolderData.EquipmentManager.Helm;

            if (equipmentData.Index != 0)
            {
                if (helm != null)
                {
                    helm.HideEqupiment(stat);
                }
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
        protected override void AddDataToStat(Stat stat)
        {
            //Adds less damage if the holder has rapid attack.
            float multiplier = 1;
            switch (HolderData.Type)
            {
                case Class.ClassType.Kibapon:
                    multiplier = 0.27f;
                    break;
                case Class.ClassType.Megapon:
                case Class.ClassType.Yumipon:
                    multiplier = 0.35f;
                    break;
                case Class.ClassType.Toripon:
                    multiplier = 0.5f;
                    break;
            }
            stat.Add(Stat, multiplier);
            HolderData.AddMass(Mass);
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
