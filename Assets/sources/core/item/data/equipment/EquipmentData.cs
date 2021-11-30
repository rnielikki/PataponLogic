using PataRoad.Core.Character;
using PataRoad.Core.Character.Equipments;
using UnityEngine;

namespace PataRoad.Core.Items
{
    /// <summary>
    /// Represents any equipment DATA that Patapon (or enemy) use, like spear, shield, cape, helm. Attached to every equipment prefab.
    /// </summary>
    [CreateAssetMenu(fileName = "equipment-data-as-number", menuName = "Item/EquipmentData/EquipmentData")]
    public class EquipmentData : ScriptableObject, IItem
    {
        public System.Guid Id { get; set; }

        [SerializeField]
        private bool _isUniqueItem;
        public bool IsUniqueItem => _isUniqueItem;
        /// <summary>
        /// The group of the equipment. - like "Bird", "Spear", "Sword" ...
        /// </summary>
        [SerializeField]
        private string _group;
        public string Group => _group;
        public int Index { get; set; }
        ///<summary>
        /// Name of the equipment, e.g. "Wooden Shield", "Divine Sword"
        /// </summary>
        [SerializeField]
        private string _name;
        public string Name => _name;
        [SerializeField]
        private string _description;
        public string Description => _description;
        /// <summary>
        /// The stat bonus that attached to the weapon.
        /// </summary>
        [SerializeReference]
        protected Stat _stat = new Stat();
        public Stat Stat => _stat;

        /// <summary>
        /// Sprite image of the weapon.
        /// </summary>
        [SerializeField]
        private Sprite _image;
        public Sprite Image => _image;

        /// <summary>
        /// Mass of the weapon. The mass affects to the knockback distance. Also it affects to the wind effect.
        /// </summary>
        /// <note>This won't affect to the throwing distance.</note>
        [SerializeField]
        private float _mass;
        public float Mass => _mass;


        [SerializeField]
        protected EquipmentType _type;
        public EquipmentType Type => _type;

        public ItemType ItemType => ItemType.Equipment;

        /// <summary>
        /// Add this value to the sprite position, to put it center.
        /// </summary>
        /// <returns>Pivot offset from center.</returns>
        public Vector2 GetPivotOffset() => new Vector2(Image.pivot.x / Image.rect.width - 0.5f, Image.pivot.y / Image.rect.height - 0.5f);
    }
}
