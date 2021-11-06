using System.Collections.Generic;
using System.Linq;

namespace PataRoad.Core.Rhythm.Command
{
    /// <summary>
    /// Contains command list, connected with <see cref="CommandTree"/>.
    /// </summary>
    public class CommandListData
    {
        private readonly CommandTree _root = new CommandTree();

        internal CommandListData(Dictionary<CommandSong, DrumType[]> commandMap)
        {
            foreach (var commandKv in commandMap)
            {
                var song = commandKv.Key;
                var command = commandKv.Value;
                if (command.Length != 4)
                {
                    throw new System.ArgumentException($"Command must be 4-beat but one or more commands are {command.Length} beat");
                }
                CreateCommandTree(_root, command, song);
            }
        }
        /// <summary>
        /// Checks if the drum beat is valid, also finds song by drum beats.
        /// </summary>
        /// <param name="drums">Drums to check.</param>
        /// <param name="song">Note that this parameter is set to <see cref="CommandSong.None"/> until the full command is ready.</param>
        /// <returns><c>true</c> if song can be continue or exists, otherwise <c>false</c>.</returns>
        /// <note>Don't rely on <paramref name="song"/> to find if the command is valid!</note>
        internal bool TryGetCommand(IEnumerable<DrumType> drums, out CommandSong song) => TryGetCommand(_root, drums, out song);
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
        private void CreateCommandTree(CommandTree parent, IEnumerable<DrumType> drums, CommandSong song)
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
            CreateCommandTree(next, drums.Skip(1), song);
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
