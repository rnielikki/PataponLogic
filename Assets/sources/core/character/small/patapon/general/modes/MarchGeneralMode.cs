using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons.General
{
    /// <summary>
    /// Immune to all status effects while marching (except tumbling).
    /// </summary>
    class MarchGeneralMode : StatChangingGeneralMode
    {
        public override CommandSong ActivationCommand => CommandSong.Patapata;
        public override void Init()
        {
            Init(3);
        }
        public override Stat CalculateStat(Stat stat)
        {
            stat.BoostResistance(UnityEngine.Mathf.Infinity);
            return stat;
        }
    }
}
