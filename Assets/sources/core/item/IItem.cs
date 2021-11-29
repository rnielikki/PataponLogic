namespace PataRoad.Core.Items
{
    /// <summary>
    /// Interface of ANY item.
    /// </summary>
    public interface IItem
    {
        /// <summary>
        /// Unique instance id IN SCENE, automatically generated by loader. DON'T USE this as saved data reference.
        /// </summary>
        public System.Guid Id { get; set; }
        /// <summary>
        /// Name of the item.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Description of the item. Will be shown on inventory window.
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// Image, that will be shown in every thumbnail.
        /// </summary>
        public UnityEngine.Sprite Image { get; }
        /// <summary>
        /// Item type, like weapon or material.
        /// </summary>
        public ItemType ItemType { get; }
        /// <summary>
        /// Group of the item. For example, this can be weapon type or material type.
        /// </summary>
        public string Group { get; }
        /// <summary>
        /// Index of the item. This can be useful for searching item data via <see cref="ItemLoader"/>.
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// <c>true</c> if item can be obtained only once in the game. Default is <c>false</c>.
        /// </summary>
        public bool IsUniqueItem { get; }
    }
}