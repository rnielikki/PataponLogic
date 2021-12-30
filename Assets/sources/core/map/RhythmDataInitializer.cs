using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Map
{
    public class RhythmDataInitializer : MonoBehaviour
    {
        [SerializeField]
        Rhythm.Command.RhythmCommand _command;
        [SerializeField]
        GameObject _songGuideParent;
        private void Awake()
        {
            var allSongs = Global.GlobalData.Inventory
                .GetKeyItems<Items.SongItemData>("Song").Select(item => item.Song).ToArray();
            _command.SetCommandSong(allSongs);

            foreach (var guide in _songGuideParent.GetComponentsInChildren<SongGuide>(true))
            {
                if ((guide.Song == Rhythm.Command.CommandSong.None && !Global.GlobalData.PataponInfo.CanUseSummon) //summon
                    || (System.Array.IndexOf(allSongs, guide.Song) < 0))
                {
                    guide.gameObject.SetActive(false);
                }
            }
        }
        void Start()
        {
            var allDrums = Global.GlobalData.Inventory
                .GetKeyItems<Items.DrumItemData>("Drum").Select(item => item.Drum);
            foreach (var input in GetComponentsInChildren<Rhythm.RhythmInput>())
            {
                if (!allDrums.Contains(input.DrumType)) input.gameObject.SetActive(false);
            }
        }
    }
}
