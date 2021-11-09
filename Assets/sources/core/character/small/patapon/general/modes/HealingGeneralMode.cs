using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons.General
{
    class HealingGeneralMode : IGeneralMode
    {
        public CommandSong ActivationCommand => CommandSong.Ponpon;

        public void Activate(PataponGroup group)
        {
            group.HealAllInGroup(50);
        }

        public void CancelGeneralMode()
        {
        }
    }
}
