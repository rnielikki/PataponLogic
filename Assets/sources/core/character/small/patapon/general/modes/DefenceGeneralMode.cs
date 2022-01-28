using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons.General
{
    class DefenceGeneralMode : StatChangingGeneralMode
    {
        public override CommandSong ActivationCommand => CommandSong.Chakachaka;
        private float _defenceMultiplier;
        private float _resistBoost;
        public override void Init()
        {
            Init(4);
            switch (Rhythm.RhythmEnvironment.Difficulty)
            {
                case Rhythm.Difficulty.Easy:
                    _defenceMultiplier = 1.8f;
                    _resistBoost = 0.2f;
                    break;
                case Rhythm.Difficulty.Normal:
                    _defenceMultiplier = 2.5f;
                    _resistBoost = 0.28f;
                    break;
                case Rhythm.Difficulty.Hard:
                    _defenceMultiplier = 3;
                    _resistBoost = 0.35f;
                    break;
            }
        }
        public override Stat CalculateStat(Stat stat)
        {
            stat.DefenceMin *= _defenceMultiplier;
            stat.DefenceMax *= _defenceMultiplier;
            stat.BoostResistance(_resistBoost);
            return stat;
        }
    }
}
