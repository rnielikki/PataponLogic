namespace PataRoad.Core.Character.Hazorons.Levels
{
    interface IHazoronStatModifier : Map.IHavingLevel
    {
        public int Level { get; }
        public void SetModifyTarget(Stat stat);
    }
}
