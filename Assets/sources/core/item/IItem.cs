namespace PataRoad.Core.Items
{
    public interface IItem
    {
        /// <summary>
        /// PREFAB PATH *RELATIVE TO RESOURCE DIR*, will be unique value. Can be also used for loading saved data.
        /// </summary>
        /// <remarks>In same scene (without game reloading), InstanceID checking is enough.</remarks>
        /// <note>If Unity regenerates GUID, using GUID will destroy save data.</note>
        public string Path { get; }
        public string Name { get; }
        public string Description { get; }
        public Character.Stat Stat { get; }
        public UnityEngine.Sprite Image { get; }
        public ItemType ItemType { get; }
    }
}
