
using PataRoad.Core.Rhythm;
using System.Collections.Generic;

namespace PataRoad.Core.Character.Bosses
{
    /// <summary>
    /// Executes boss action, including combo action, even player stopped entering command.
    /// </summary>
    public class BossTurnManager
    {
        private readonly Queue<string> _actionQueue = new Queue<string>();
        private int _turnCount;
        public bool Attacking { get; private set; }
        public bool IsEmpty => _actionQueue.Count == 0;
        public UnityEngine.Events.UnityEvent OnAttackEnd { get; } = new UnityEngine.Events.UnityEvent();
        private readonly CharacterAnimator _charAnimator;
        private string _current;
        private bool _willAttackEnd;
        internal BossTurnManager(CharacterAnimator charAnimator)
        {
            _charAnimator = charAnimator;
        }
        //-- start and end
        /// <summary>
        /// Starts the boss attack from queue. This ends when queue ends.
        /// </summary>
        public void StartAttack()
        {
            if (Attacking) return;
            Attacking = true;
            _turnCount = 0;
            Rhythm.Command.TurnCounter.OnNextBossTurn.AddListener(() => RhythmTimer.OnTime.AddListener(CountTurn));
        }
        public void End()
        {
            _actionQueue.Clear();
            RhythmTimer.OnTime.RemoveListener(CountTurn);
            Attacking = false;
        }
        // -- normal actions
        public void SetOneAction(string actionName)
        {
            if (Attacking) return;
            _actionQueue.Enqueue(actionName);
        }
        //-- combo
        public void SetComboAttack(IEnumerable<string> actions)
        {
            if (Attacking) return;
            foreach (var action in actions) _actionQueue.Enqueue(action);
        }

        private void CountTurn()
        {
            switch (_turnCount)
            {
                case 0:
                    if (_actionQueue.Count != 0)
                    {
                        _current = _actionQueue.Dequeue();
                        _charAnimator.Animate(_current + "-before");
                    }
                    else
                    {
                        _willAttackEnd = true;
                    }
                    break;
                case 2:
                    if (_willAttackEnd)
                    {
                        Attacking = false;
                        OnAttackEnd.Invoke();
                        OnAttackEnd.RemoveAllListeners();
                        RhythmTimer.OnTime.RemoveListener(CountTurn);
                        _willAttackEnd = false;
                    }
                    break;
                case 6:
                    _charAnimator.Animate(_current);
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
