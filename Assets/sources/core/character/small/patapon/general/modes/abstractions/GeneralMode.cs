namespace PataRoad.Core.Character.Patapons.General
{
    /// <summary>
    /// Defines one general mode behaviour.
    /// </summary>
    public abstract class GeneralMode : UnityEngine.MonoBehaviour
    {
        /// <summary>
        /// The perfect command to activate. E.g. patapata, ponpon...
        /// </summary>
        public abstract Rhythm.Command.CommandSong ActivationCommand { get; }
        /// <summary>
        /// Behaviour, when General Mode is called from perfect hit.
        /// </summary>
        /// <param name="group"></param>
        public abstract void Activate(PataponGroup group);
        /// <summary>
        /// NOTE: This is called for COMMAND CANCEL MODE. Doesn't affect when only perfect hit lost in fever combo. You should define it manually if you want.
        /// </summary>
        public abstract void CancelGeneralMode();
        /// <summary>
        /// Initialise General Mode in first time, before use. Expected to be called only once.
        /// </summary>
        public abstract void Init();
    }
}
