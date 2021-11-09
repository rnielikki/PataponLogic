using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons.General
{
    class AttackGeneralMode : StatChangingGeneralMode
    {
        public override CommandSong ActivationCommand => CommandSong.Ponpon;
        internal AttackGeneralMode() : base(5)
        {
        }
        public override Stat CalculateStat(Stat stat)
        {
            stat.AddDamage(20);
            return stat;
        }
    }
}
