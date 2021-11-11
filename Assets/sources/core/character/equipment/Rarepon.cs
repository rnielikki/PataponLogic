using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    public class Rarepon : Equipment
    {
        public Stat Stat { get; private set; }
        public bool IsNormal { get; private set; } = true;
        [SerializeField]
        [Tooltip("It can be eye color, and Megapon mouth color etc.")]
        private SpriteRenderer[] _spritesToChangeColor;
        private void Awake()
        {
            Load();
            _type = EquipmentType.Rarepon;
            if (Stat == null) Stat = new Stat();
        }

        protected override void ReplaceImage(EquipmentData equipmentData)
        {
            base.ReplaceImage(equipmentData);
            var color = equipmentData.GetComponent<SpriteRenderer>().color;
            IsNormal = false;
            _spriteRenderers[0].color = color;

            foreach (var renderer in _spritesToChangeColor)
            {
                renderer.color = color;
            }
            switch (Holder.Class)
            {
                case Patapons.ClassType.Dekapon:
                    //resize it.
                    break;
                case Patapons.ClassType.Mahopon:
                case Patapons.ClassType.Megapon:
                    //Hide hat.
                    break;
            }
        }
    }
}
