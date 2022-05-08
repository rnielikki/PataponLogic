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

        private float _multiplier = 1;

        private void Start()
        {
            Load();
        }
        internal override void Load()
        {
            base.Load();
            _multiplier = GetMultiplier();
        }
        protected override EquipmentData GetDefault() => Global.GlobalData.CurrentSlot.RareponInfo.DefaultRarepon.Data;
        internal override void ReplaceEqupiment(EquipmentData equipmentData, Stat stat)
        {
            if (HolderData == null) Load();
            var helm = HolderData.EquipmentManager.Helm;

            if (equipmentData.Index != 0) //NON-NORMAL
            {
                if (helm != null) // non-normal rarepon, helm
                {
                    helm.HideEqupiment(stat);
                }
                if (_spriteToHideOnRarepon != null) _spriteToHideOnRarepon.enabled = false;
            }
            else if (helm != null) //NORMAL rarepon, non-helm
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
            stat.Add(Stat, _multiplier);
            HolderData.AddMass(Mass);
        }
        protected override void RemoveDataFromStat(Stat stat)
        {
            if (CurrentData == null) return;
            stat.Subtract(Stat, _multiplier);
            HolderData.AddMass(-Mass);
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
        private float GetMultiplier()
        {
            switch (HolderData.Type)
            {
                case Class.ClassType.Kibapon:
                    return 0.27f;
                case Class.ClassType.Megapon:
                case Class.ClassType.Yumipon:
                    return 0.35f;
                case Class.ClassType.Toripon:
                    return 0.5f;
                default:
                    return 1;
            }
        }
    }
}
