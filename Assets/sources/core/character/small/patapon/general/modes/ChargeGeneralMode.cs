using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons.General
{
    class ChargeGeneralMode : StatChangingGeneralMode
    {
        public override CommandSong ActivationCommand => CommandSong.Ponchaka;
        private float _adder;

        public override void Init()
        {
            Init(2);
            switch (Rhythm.RhythmEnvironment.Difficulty)
            {
                case Rhythm.Difficulty.Easy:
                    _adder = 0.6f;
                    break;
                case Rhythm.Difficulty.Normal:
                    _adder = 0.875f;
                    break;
                case Rhythm.Difficulty.Hard:
                    _adder = 1f;
                    break;
            }
        }
        public override Stat CalculateStat(Stat stat)
        {
            stat.Critical += _adder;
            stat.CriticalResistance = UnityEngine.Mathf.Infinity;
            stat.KnockbackResistance += _adder * 0.5f;
            return stat;
        }
    }
}
