using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    public class Helm : Equipment
    {
        private Sprite _defaultImage;
        private void Awake()
        {
            Load();
            _defaultImage = _spriteRenderers[0].sprite;
        }
        internal override void ReplaceEqupiment(EquipmentData equipmentData, Stat stat)
        {
            if (!Holder.EquipmentManager.Rarepon.IsNormal) return;
            base.ReplaceEqupiment(equipmentData, stat);
        }
        internal void HideEqupiment(Stat stat)
        {
            ReplaceImage(null);
            RemoveDataFromStat(stat);
        }
        internal void ShowEqupiment()
        {
            _spriteRenderers[0].sprite = _defaultImage;
        }
    }
}
