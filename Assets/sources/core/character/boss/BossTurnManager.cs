
using PataRoad.Core.Rhythm;
using System.Collections.Generic;

namespace PataRoad.Core.Character.Bosses
{
    /// <summary>
    /// Executes boss action, including combo action, even player stopped entering command.
    /// </summary>
    public class BossTurnManager
    {
        private readonly Queue<UnityEngine.Events.UnityAction> _actionQueue = new Queue<UnityEngine.Events.UnityAction>();
        private int _turnCount;
        public bool Attacking { get; private set; }
        public bool IsEmpty => _actionQueue.Count == 0;
        public UnityEngine.Events.UnityEvent OnAttackEnd { get; } = new UnityEngine.Events.UnityEvent();
        internal BossTurnManager()
        {
        }
        //-- start and end
        /// <summary>
        /// Starts the boss attack from queue. This ends when queue ends.
        /// </summary>
        public void StartAttack()
        {
            Attacking = true;
            _turnCount = 0;
            Rhythm.Command.TurnCounter.OnNextNonPlayerTurn.AddListener(() => RhythmTimer.OnTime.AddListener(CountTurn));
        }
        public void End()
        {
            Attacking = false;
            _actionQueue.Clear();
            RhythmTimer.OnTime.RemoveListener(CountTurn);
        }
        // -- normal actions
        public BossTurnManager SetOneAction(UnityEngine.Events.UnityAction action)
        {
            if (Attacking) return null;
            _actionQueue.Enqueue(action);
            return this;
        }
        //-- combo
        public BossTurnManager SetComboAttack(IEnumerable<UnityEngine.Events.UnityAction> actions)
        {
            if (Attacking) return null;
            foreach (var action in actions) _actionQueue.Enqueue(action);
            return this;
        }

        private void CountTurn()
        {
            switch (_turnCount)
            {
                case 2:
                    if (_actionQueue.Count != 0) _actionQueue.Dequeue().Invoke();
                    else
                    {
                        Attacking = false;
                        OnAttackEnd.Invoke();
                        OnAttackEnd.RemoveAllListeners();
                        RhythmTimer.OnTime.RemoveListener(CountTurn);
                    }
                    break;
            }
            _turnCount = (_turnCount + 1) % 8;
        }
        public void Destroy()
        {
            End();
            OnAttackEnd.RemoveAllListeners();
        }
    }
}
