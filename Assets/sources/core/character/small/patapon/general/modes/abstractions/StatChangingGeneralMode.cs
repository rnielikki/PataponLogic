using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons.General
{
    abstract class StatChangingGeneralMode : IGeneralMode
    {
        public abstract CommandSong ActivationCommand { get; }
        protected IStatOperation _operation;
        private PataponGroup _group;
        private readonly int _activeTurn;
        private int _leftActiveTurnCount;

        protected StatChangingGeneralMode(int activeTurn = 1)
        {
            _activeTurn = activeTurn;
            _operation = new GeneralModeStatOperation(this);
        }

        public void Activate(PataponGroup group)
        {
            if (_leftActiveTurnCount > 0)
            {
                _leftActiveTurnCount++;
                return;
            }
            _group = group;
            foreach (var patapon in group.Patapons)
            {
                patapon.StatOperator.Add(_operation);
            }
            DeactivateAfterTurns();
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
            foreach (var patapon in _group.Patapons)
            {
                patapon.StatOperator.Remove(_operation);
            }
        }
        public abstract Stat CalculateStat(Stat stat);

        public void CancelGeneralMode()
        {
            _leftActiveTurnCount = 0;
            if (_group == null) return;
            Deactivate();
            TurnCounter.OnTurn.RemoveListener(CountUntilDeactive);
        }
    }
}
