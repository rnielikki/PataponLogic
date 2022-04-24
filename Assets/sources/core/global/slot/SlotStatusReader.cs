using System.Collections.Generic;
using System.Linq;

namespace PataRoad.Core.Global.Slots
{
    /// <summary>
    /// Reads and returns all available data of specific category, like drum or class.
    /// </summary>
    public static class SlotStatusReader
    {
        private static Slot _slot => GlobalData.CurrentSlot;
        public static IEnumerable<Character.Class.ClassType> GetAvailableClasses()
        {
            return _slot.Inventory
                .GetKeyItems<Items.ClassMemoryData>("Class")
                .Select(item => item.Class);
        }
        public static IEnumerable<Rhythm.DrumType> GetAvailableDrums()
        {
            return _slot.Inventory
               .GetKeyItems<Items.DrumItemData>("Drum").Select(item => item.Drum);
        }
        public static IEnumerable<Rhythm.Command.CommandSong> GetAvailableSongs()
        {
            return _slot.Inventory
                .GetKeyItems<Items.SongItemData>("Song").Select(item => item.Song);
        }
    }
}