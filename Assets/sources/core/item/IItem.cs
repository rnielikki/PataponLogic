namespace PataRoad.Core.Items
{
    public interface IItem
    {
        public string Name { get; }
        public string Description { get; }
        public Character.Stat Stat { get; }
        public UnityEngine.Sprite Image { get; }
    }
}
