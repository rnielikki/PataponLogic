namespace PataRoad.Core.Character.Patapons.General
{
    public abstract class GeneralMode : UnityEngine.MonoBehaviour
    {
        public abstract Rhythm.Command.CommandSong ActivationCommand { get; }
        public abstract void Activate(PataponGroup group);
        public abstract void CancelGeneralMode();
        public abstract void Init();
    }
}
