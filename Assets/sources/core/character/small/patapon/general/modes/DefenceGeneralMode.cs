using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons.General
{
    class DefenceGeneralMode : StatChangingGeneralMode
    {
        public override CommandSong ActivationCommand => CommandSong.Chakachaka;
        public override void Init() => Init(3);
        public override Stat CalculateStat(Stat stat)
        {
            stat.DefenceMin *= 4;
            stat.DefenceMax *= 4;
            stat.BoostResistance(0.3f);
            return stat;
        }
    }
}
