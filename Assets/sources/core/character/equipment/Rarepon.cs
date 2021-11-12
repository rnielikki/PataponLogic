using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    public class Rarepon : Equipment
    {
        public bool IsNormal { get; private set; } = true;
        [SerializeField]
        [Tooltip("It can be eye color, and Megapon mouth color etc.")]
        private SpriteRenderer[] _spritesToChangeColor;
        [SerializeField]
        private SpriteRenderer _spriteToHideOnRarepon;
        private void Awake()
        {
            Load();
        }
        internal override void ReplaceEqupiment(EquipmentData equipmentData, Stat stat)
        {
            if (_currentData == null)
            {
                var helm = Holder.EquipmentManager.Helm;
                if (helm != null)
                {
                    helm.HideEqupiment(stat);
                }
            }
            base.ReplaceEqupiment(equipmentData, stat);
        }

        protected override void ReplaceImage(EquipmentData equipmentData)
        {
            base.ReplaceImage(equipmentData);
            var color = equipmentData.GetComponent<SpriteRenderer>().color;
            IsNormal = false;

            Changecolor(color);

            if (_spriteToHideOnRarepon != null) _spriteToHideOnRarepon.enabled = false;
            if (Holder.Class == Patapons.ClassType.Dekapon)
            {
                //resize it.
            }
        }
        public void SetToNormal(Stat stat)
        {
            if (_currentData == null) return;
            Changecolor(Color.white);
            _currentData = null;
            if (_spriteToHideOnRarepon != null) _spriteToHideOnRarepon.enabled = true;
            var helm = Holder.EquipmentManager.Helm;
            if (helm != null)
            {
                helm.ShowEqupiment();
            }
            base.ReplaceImage(null);
            RemoveDataFromStat(stat);
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
