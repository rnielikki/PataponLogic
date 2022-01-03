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
        [Header("drum guides")]
        [SerializeField]
        GameObject _pataDrumGuide;
        [SerializeField]
        GameObject _ponDrumGuide;
        [SerializeField]
        GameObject _chakaDrumGuide;
        [SerializeField]
        GameObject _donDrumGuide;
        private void Awake()
        {
            var allSongs = Global.GlobalData.CurrentSlot.Inventory
                .GetKeyItems<Items.SongItemData>("Song").Select(item => item.Song).ToArray();
            _command.SetCommandSong(allSongs);

            foreach (var guide in _songGuideParent.GetComponentsInChildren<SongGuide>(true))
            {
                if ((guide.Song == Rhythm.Command.CommandSong.None && !Global.GlobalData.CurrentSlot.PataponInfo.CanUseSummon) //summon
                    || (System.Array.IndexOf(allSongs, guide.Song) < 0))
                {
                    guide.gameObject.SetActive(false);
                }
            }
        }
        void Start()
        {
            var allDrums = Global.GlobalData.CurrentSlot.Inventory
                .GetKeyItems<Items.DrumItemData>("Drum").Select(item => item.Drum);
            foreach (var input in GetComponentsInChildren<Rhythm.RhythmInput>())
            {
                if (!allDrums.Contains(input.DrumType))
                {
                    input.gameObject.SetActive(false);
                    switch (input.DrumType)
                    {
                        case Rhythm.DrumType.Pata:
                            _pataDrumGuide.SetActive(false);
                            break;
                        case Rhythm.DrumType.Pon:
                            _ponDrumGuide.SetActive(false);
                            break;
                        case Rhythm.DrumType.Chaka:
                            _chakaDrumGuide.SetActive(false);
                            break;
                        case Rhythm.DrumType.Don:
                            _donDrumGuide.SetActive(false);
                            break;
                    }
                }
            }
        }
    }
}
