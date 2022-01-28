using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons.General
{
    class HealingGeneralMode : GeneralMode
    {
        public override CommandSong ActivationCommand => CommandSong.Donchaka;
        private int _healAmount;
        public override void Init()
        {
            //This is no Patapon3 so you don't need almost every time perfect
            switch (Rhythm.RhythmEnvironment.Difficulty)
            {
                case Rhythm.Difficulty.Easy:
                    _healAmount = 20;
                    break;
                case Rhythm.Difficulty.Normal:
                    _healAmount = 45;
                    break;
                case Rhythm.Difficulty.Hard:
                    _healAmount = 60;
                    break;
            }

        }
        public override void Activate(PataponGroup group)
        {
            group.HealAllInGroup(_healAmount);
        }
        public override void CancelGeneralMode()
        {
            //It's just healing, nothing to cancel!
        }
    }
}
