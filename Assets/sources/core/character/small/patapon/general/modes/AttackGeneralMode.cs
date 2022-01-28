using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons.General
{
    class AttackGeneralMode : StatChangingGeneralMode
    {
        public override CommandSong ActivationCommand => CommandSong.Ponpon;
        private float _damageMultiplier;
        public override void Init()
        {
            Init(4);
            //what's the even point to play easy if you'll activate whole time hero mode
            switch (Rhythm.RhythmEnvironment.Difficulty)
            {
                case Rhythm.Difficulty.Easy:
                    _damageMultiplier = 1.6f;
                    break;
                case Rhythm.Difficulty.Normal:
                    _damageMultiplier = 3;
                    break;
                case Rhythm.Difficulty.Hard:
                    _damageMultiplier = 4;
                    break;
            }
        }
        public override Stat CalculateStat(Stat stat)
        {
            stat.MultipleDamage(_damageMultiplier);
            return stat;
        }
    }
}
