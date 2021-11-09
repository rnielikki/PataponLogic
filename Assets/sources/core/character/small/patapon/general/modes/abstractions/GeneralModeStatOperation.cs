namespace PataRoad.Core.Character.Patapons.General
{
    internal class GeneralModeStatOperation : IStatOperation
    {
        private StatChangingGeneralMode _mode;
        internal GeneralModeStatOperation(StatChangingGeneralMode mode)
        {
            _mode = mode;
        }
        public Stat Calculate(Stat input) => _mode.CalculateStat(input);
    }
}
