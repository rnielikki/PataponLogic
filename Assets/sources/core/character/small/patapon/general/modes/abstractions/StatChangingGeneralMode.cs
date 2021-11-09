using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons.General
{
    abstract class StatChangingGeneralMode : IGeneralMode
    {
        public abstract CommandSong ActivationCommand { get; }
        protected IStatOperation _operation;
        private readonly System.Collections.Generic.List<PataponGroup> _groups = new System.Collections.Generic.List<PataponGroup>();
        private readonly int _activeTurn;
        private int _leftActiveTurnCount;

        protected StatChangingGeneralMode(int activeTurn = 1)
        {
            _activeTurn = activeTurn;
            _operation = new GeneralModeStatOperation(this);
        }

        public void Activate(PataponGroup group)
        {
            if (_groups.Contains(group)) return;

            _groups.Add(group);
            foreach (var patapon in group.Patapons)
            {
                patapon.StatOperator.Add(_operation);
            }
            if (_leftActiveTurnCount > 0)
            {
                _leftActiveTurnCount++;
            }
            else
            {
                DeactivateAfterTurns();
            }
        }

        private void DeactivateAfterTurns()
        {
            _leftActiveTurnCount = _activeTurn + 1;
            TurnCounter.OnTurn.AddListener(CountUntilDeactive);
        }

        private void CountUntilDeactive()
        {
            if (TurnCounter.IsPlayerTurn) return;
            _leftActiveTurnCount--;
            if (_leftActiveTurnCount < 1)
            {
                Deactivate();
                TurnCounter.OnTurn.RemoveListener(CountUntilDeactive);
            }
        }

        private void Deactivate()
        {
            foreach (var group in _groups)
            {
                foreach (var patapon in group.Patapons)
                {
                    patapon.StatOperator.Remove(_operation);
                }
            }
            _groups.Clear();
        }
        public abstract Stat CalculateStat(Stat stat);

        public void CancelGeneralMode()
        {
            _leftActiveTurnCount = 0;
            Deactivate();
            TurnCounter.OnTurn.RemoveListener(CountUntilDeactive);
        }
    }
}
