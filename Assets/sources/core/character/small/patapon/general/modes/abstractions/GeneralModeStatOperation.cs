namespace PataRoad.Core.Character.Patapons.General
{
    internal class GeneralModeStatOperation : IStatOperation
    {
        private readonly StatChangingGeneralMode _mode;
        internal GeneralModeStatOperation(StatChangingGeneralMode mode)
        {
            _mode = mode;
        }
        public Stat Calculate(Rhythm.Command.CommandSong song, bool charged, Stat input) => _mode.CalculateStat(input);
    }
}
