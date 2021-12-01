using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    public class Helm : Equipment
    {
        private Sprite _defaultImage;

        protected override EquipmentType _type => EquipmentType.Helm;

        private void Start()
        {
            _defaultImage = _spriteRenderers[0].sprite;
            Load();
        }
        internal override void ReplaceEqupiment(EquipmentData equipmentData, Stat stat)
        {
            if (HolderData == null) HolderData = GetComponentInParent<SmallCharacterData>();
            if (!(HolderData.EquipmentManager.Rarepon?.IsNormal ?? false)) return;
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
