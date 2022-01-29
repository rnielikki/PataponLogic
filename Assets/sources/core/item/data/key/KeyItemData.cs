using UnityEngine;

namespace PataRoad.Core.Items
{
    /// <summary>
    /// Represents any equipment DATA that Patapon (or enemy) use, like spear, shield, cape, helm. Attached to every equipment prefab.
    /// </summary>
    [CreateAssetMenu(fileName = "key-data", menuName = "Item/KeyItemData/KeyItemData")]
    public class KeyItemData : ScriptableObject, IItem
    {
        public System.Guid Id { get; set; }
        public bool IsUniqueItem => true;
        /// <summary>
        /// The group of the equipment. - like "Bird", "Spear", "Sword" ...
        /// </summary>
        [SerializeField]
        private string _group;
        public virtual string Group => _group;
        [SerializeField]
        private int _index;
        public int Index => _index;
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
        /// Sprite image of the weapon.
        /// </summary>
        [SerializeField]
        private Sprite _image;
        public Sprite Image => _image;

        public ItemType ItemType => ItemType.Key;
    }
}
