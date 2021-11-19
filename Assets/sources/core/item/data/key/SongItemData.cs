using UnityEngine;

namespace PataRoad.Core.Items
{
    /// <summary>
    /// Represents any equipment DATA that Patapon (or enemy) use, like spear, shield, cape, helm. Attached to every equipment prefab.
    /// </summary>
    [CreateAssetMenu(fileName = "drum", menuName = "KeyItemData/Song")]
    public class SongItemData : KeyItemData
    {
        public override string Group => "Song";
        [SerializeField]
        private Rhythm.Command.CommandSong _song;
        public Rhythm.Command.CommandSong Song => _song;
    }
}
