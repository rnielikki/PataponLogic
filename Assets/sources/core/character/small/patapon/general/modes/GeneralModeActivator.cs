using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons.General
{
    internal class GeneralModeActivator
    {
        private readonly IGeneralMode _generalMode;
        public CommandSong GeneralModeSong { get; }
        private readonly PataponGroup _group;
        internal GeneralModeActivator(IGeneralMode generalMode, PataponGroup group)
        {
            _generalMode = generalMode;
            GeneralModeSong = generalMode.ActivationCommand;
            _group = group;
        }
        internal void Activate()
        {
            //Will add, if it's combo,  we'll do for alllll!
            _generalMode.Activate(_group);
        }
    }
}
