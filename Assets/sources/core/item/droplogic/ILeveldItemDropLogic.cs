namespace PataRoad.Core.Items
{
    public interface ILeveldItemDropLogic
    {
        public void SetItemGroup(string group);
        public int GetAmount();
        public IItem GetItem(float levelRatio);
    }
}