using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons.General
{
    class DefenceGeneralMode : StatChangingGeneralMode
    {
        public override CommandSong ActivationCommand => CommandSong.Chakachaka;
        internal DefenceGeneralMode() : base(2)
        {
        }
        public override Stat CalculateStat(Stat stat)
        {
            stat.Defence *= 5;
            return stat;
        }
    }
}
