namespace PataRoad.Core.Items
{
    /// <summary>
    /// Represents item meatadata for indexing. Can be serialized to save.
    /// </summary>
    [System.Serializable]
    public class ItemMetadata
    {
        /// <summary>
        /// Item Type, like equipment ork key item.
        /// </summary>
        [UnityEngine.SerializeField]
        private ItemType _type;
        public ItemType Type
        {
            get => _type;

            set => _type = value;
        }

        /// <summary>
        /// Group. Material type or equipment type falls in this category.
        /// </summary>
        [UnityEngine.SerializeField]
        private string _group;
        public string Group
        {
            get => _group;

            set => _group = value;
        }

        /// <summary>
        /// Unique index of the item. For weapon, usually "better weapon" has higher index.
        /// </summary>
        [UnityEngine.SerializeField]
        private int _index;
        public int Index
        {
            get => _index;

            set => _index = value;
        }
    }
}
