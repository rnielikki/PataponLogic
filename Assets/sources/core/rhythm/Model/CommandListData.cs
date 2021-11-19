using System.Collections.Generic;
using System.Linq;

namespace PataRoad.Core.Rhythm.Command
{
    /// <summary>
    /// Contains command list, connected with <see cref="CommandTree"/>.
    /// </summary>
    public class CommandListData
    {
        protected readonly Dictionary<CommandSong, DrumType[]> _fullMap = new Dictionary<CommandSong, DrumType[]>()
        {
            { CommandSong.Patapata, new DrumType[]{ DrumType.Pata, DrumType.Pata, DrumType.Pata, DrumType.Pon }}, //PATA PATA PATA PON
            { CommandSong.Ponpon, new DrumType[]{ DrumType.Pon, DrumType.Pon, DrumType.Pata, DrumType.Pon } }, //PON PON PATA PON
            { CommandSong.Chakachaka, new DrumType[]{ DrumType.Chaka, DrumType.Chaka, DrumType.Pata, DrumType.Pon } }, //CHAKA CHAKA PATA PON
            { CommandSong.Ponpata, new DrumType[]{ DrumType.Pon, DrumType.Pata, DrumType.Pon, DrumType.Pata } } ,//PON PATA PON PATA
            { CommandSong.Ponchaka, new DrumType[]{ DrumType.Pon, DrumType.Pon, DrumType.Chaka, DrumType.Chaka } } ,//PON PON CHAKA CHAKA
            { CommandSong.Dondon, new DrumType[]{ DrumType.Don, DrumType.Don, DrumType.Chaka, DrumType.Chaka } } ,//DON DON CHAKA CHAKA
            { CommandSong.Donchaka, new DrumType[]{ DrumType.Pata, DrumType.Pon, DrumType.Don, DrumType.Chaka } } ,//PATA PON DON CHAKA
            { CommandSong.Patachaka, new DrumType[]{ DrumType.Pata, DrumType.Chaka, DrumType.Pata, DrumType.Pon } }//PATA CHAKA PATA PON (NEW): *DOES NOTHING*
        };
        private CommandSong[] _availableSongs;
        internal CommandSong[] AvailableSongs => _availableSongs;

        private readonly CommandTree _root = new CommandTree();

        internal CommandListData(CommandSong[] songs)
        {
            _availableSongs = songs;
            foreach (var song in songs) AddSong(song);
        }
        internal void AddSong(CommandSong song)
        {
            //A bit tricky but this is rare case.
            var len = _availableSongs.Length;
            System.Array.Resize(ref _availableSongs, len + 1);
            _availableSongs[len] = song;
            CreateCommandTree(_root, song, _fullMap[song]);
        }
        /// <summary>
        /// Checks if the drum beat is valid, also finds song by drum beats.
        /// </summary>
        /// <param name="drums">Drums to check.</param>
        /// <param name="song">Note that this parameter is set to <see cref="CommandSong.None"/> until the full command is ready.</param>
        /// <returns><c>true</c> if song can be continue or exists, otherwise <c>false</c>.</returns>
        /// <note>Don't rely on <paramref name="song"/> to find if the command is valid!</note>
        internal virtual bool TryGetCommand(IEnumerable<DrumType> drums, out CommandSong song) => TryGetCommand(_root, drums, out song);
        private bool TryGetCommand(CommandTree parent, IEnumerable<DrumType> drums, out CommandSong song)
        {
            song = CommandSong.None;
            if (parent.Next == null)
            {
                song = parent.Song;
                return true;
            }
            else if (!drums.Any())
            {
                song = CommandSong.None;
                return true;
            }
            if (parent.Next.TryGetValue(drums.First(), out CommandTree next))
            {
                return TryGetCommand(next, drums.Skip(1), out song);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Creates tree structure from 4-beat command.
        /// </summary>
        /// <returns>Root of the <see cref="CommandTree"/></returns>
        private void CreateCommandTree(CommandTree parent, CommandSong song, IEnumerable<DrumType> drums)
        {
            if (!drums.Any())
            {
                parent.Next = null;
                parent.Song = song;
                return;
            }
            DrumType firstDrum = drums.First();
            CommandTree next;
            if (!parent.Next.TryGetValue(firstDrum, out next))
            {
                next = new CommandTree();
                parent.Next.Add(firstDrum, next);
            }
            CreateCommandTree(next, song, drums.Skip(1));
        }
        /// <summary>
        /// Represents tree structure from list of commands. End of the tree always should include <see cref="CommandSong"/>. Shouldn't be used/exposed outside here.
        /// </summary>
        private class CommandTree
        {
            internal Dictionary<DrumType, CommandTree> Next { get; set; } = new Dictionary<DrumType, CommandTree>();
            internal CommandSong Song { get; set; } //only valid if CommandTree.Next is null
        }
    }
}
