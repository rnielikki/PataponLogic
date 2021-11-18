using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons.General
{
    class HealingGeneralMode : GeneralMode
    {
        public override CommandSong ActivationCommand => CommandSong.Donchaka;
        public override void Init()
        {
            //Simple healing. Nothing to initialise.
        }
        public override void Activate(PataponGroup group)
        {
            group.HealAllInGroup(50);
        }
        public override void CancelGeneralMode()
        {
            //It's just healing, nothing to cancel!
        }
    }
}
