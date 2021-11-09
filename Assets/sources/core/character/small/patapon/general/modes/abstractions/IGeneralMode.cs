
namespace PataRoad.Core.Character.Patapons.General
{
    public interface IGeneralMode
    {
        public Rhythm.Command.CommandSong ActivationCommand { get; }
        public void Activate(PataponGroup group);
        public void CancelGeneralMode();
    }
}
