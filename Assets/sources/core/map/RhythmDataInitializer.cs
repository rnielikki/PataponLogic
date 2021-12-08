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
            var allSongs = GlobalData.Inventory
                .GetKeyItems<Items.SongItemData>("Song").Select(item => item.Song);
            _command.SetCommandSong(allSongs.ToArray());
        }
        void Start()
        {
            var allDrums = GlobalData.Inventory
                .GetKeyItems<Items.DrumItemData>("Drum").Select(item => item.Drum);
            foreach (var input in GetComponentsInChildren<Rhythm.RhythmInput>())
            {
                if (!allDrums.Contains(input.DrumType)) input.gameObject.SetActive(false);
            }
        }
    }
}
