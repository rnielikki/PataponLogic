using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Map
{
    public class RhythmDataInitializer : MonoBehaviour
    {
        [SerializeField]
        Rhythm.Command.RhythmCommand _command;
        private void Awake()
        {
            var songs = GlobalData.Inventory.GetItemsByType(Items.ItemType.Key, "Song");
            var allSongs = songs.Select(drm => (drm.item as Items.SongItemData).Song);
            _command.SetCommandSong(allSongs.ToArray());
        }
        void Start()
        {
            var drums = GlobalData.Inventory.GetItemsByType(Items.ItemType.Key, "Drum");
            var allDrums = drums.Select(drm => (drm.item as Items.DrumItemData).Drum);
            foreach (var input in GetComponentsInChildren<Rhythm.RhythmInput>())
            {
                if (!allDrums.Contains(input.DrumType)) input.gameObject.SetActive(false);
            }
        }
    }
}
