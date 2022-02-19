using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons.General
{
    /// <summary>
    /// Immune to all status effects while marching (except tumbling).
    /// </summary>
    class MarchGeneralMode : StatChangingGeneralMode
    {
        public override CommandSong ActivationCommand => CommandSong.Patapata;
        private float _defenceMultiplier;
        public override void Init()
        {
            Init(3);
            switch (Rhythm.RhythmEnvironment.Difficulty)
            {
                case Rhythm.Difficulty.Easy:
                    _defenceMultiplier = 1;
                    break;
                case Rhythm.Difficulty.Normal:
                    _defenceMultiplier = 1.5f;
                    break;
                case Rhythm.Difficulty.Hard:
                    _defenceMultiplier = 2;
                    break;
            }
        }
        public override Stat CalculateStat(Stat stat)
        {
            stat.BoostResistance(UnityEngine.Mathf.Infinity);
            stat.DefenceMin *= _defenceMultiplier;
            stat.DefenceMax *= _defenceMultiplier;
            return stat;
        }
    }
}
