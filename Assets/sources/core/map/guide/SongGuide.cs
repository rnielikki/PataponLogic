using UnityEngine;

namespace PataRoad.Core.Map
{
    class SongGuide : MonoBehaviour
    {
        [SerializeField]
        Rhythm.Command.CommandSong _song;
        public Rhythm.Command.CommandSong Song => _song;
    }
}
