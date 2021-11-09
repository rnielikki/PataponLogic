using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons.General
{
    internal class GeneralModeActivator
    {
        private readonly IGeneralMode _generalMode;
        public CommandSong GeneralModeSong { get; }
        private readonly PataponGroup _group;
        internal bool OnGeneralModeCombo { get; private set; }

        internal GeneralModeActivator(IGeneralMode generalMode, PataponGroup group)
        {
            _generalMode = generalMode;
            GeneralModeSong = generalMode.ActivationCommand;
            _group = group;
            TurnCounter.OnTurn.AddListener(() =>
            {
                if (!TurnCounter.IsPlayerTurn)
                {
                    OnGeneralModeCombo = false;
                }
            });
        }
        internal void Activate(System.Collections.Generic.IEnumerable<PataponGroup> groups)
        {
            if (OnGeneralModeCombo)
            {
                foreach (var group in groups)
                {
                    _generalMode.Activate(group);
                }
            }
            else
            {
                _generalMode.Activate(_group);
            }
            TurnCounter.OnNextTurn.AddListener(() =>
            {
                OnGeneralModeCombo = true;
            });
        }
        internal void Cancel()
        {
            OnGeneralModeCombo = false;
            _generalMode.CancelGeneralMode();
        }
    }
}
