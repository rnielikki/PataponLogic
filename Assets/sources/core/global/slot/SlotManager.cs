namespace PataRoad.Core.Global.Slots
{
    public class SlotManager
    {
        public static Slot CurrentSlot { get; set; }
        internal SlotManager()
        {
            SlotMetaList.Load();
        }
        public void LoadSlot(Slot slot)
        {
            CurrentSlot = slot;
        }
        public SlotMeta SaveSlot(int index) => CurrentSlot.Save(index);
        public void UnloadSlot()
        {
            CurrentSlot = null;
        }
    }
}
