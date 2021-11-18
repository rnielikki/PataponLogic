namespace PataRoad.Core.Character
{
    /// <summary>
    /// Represents any stat pipeline calculation.
    /// </summary>
    public interface IStatOperation
    {
        public Stat Calculate(Rhythm.Command.CommandSong song, bool charged, Stat input);
    }
}
