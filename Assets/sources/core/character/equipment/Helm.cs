using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    public class Helm : Equipment
    {
        private Sprite _defaultImage;
        private void Awake()
        {
            Load();
            _type = EquipmentType.Helm;
            _defaultImage = _spriteRenderers[0].sprite;
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
