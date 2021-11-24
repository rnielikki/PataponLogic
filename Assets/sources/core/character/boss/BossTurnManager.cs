
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
        private readonly BossAttackData _data;
        internal BossTurnManager(BossAttackData data)
        {
            _data = data;
            _charAnimator = data.CharAnimator;
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
            if (_actionQueue.Count > 1)
            {
                Rhythm.Command.TurnCounter.OnNonPlayerTurn.AddListener(WillStartComboAttack);
            }
            else
            {
                Rhythm.Command.TurnCounter.OnNonPlayerTurn.AddListener(CountSingleAttack);
            }
        }
        public void End()
        {
            _actionQueue.Clear();
            RhythmTimer.OnTime.RemoveListener(CountSingleTurn);
            RhythmTimer.OnTime.RemoveListener(CountComboTurn);
            Rhythm.Command.TurnCounter.OnNonPlayerTurn.RemoveListener(CountSingleAttack);
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
        private void WillStartComboAttack()
        {
            RhythmTimer.OnNext.AddListener(() => RhythmTimer.OnTime.AddListener(CountComboTurn));
        }

        private void CountSingleAttack()
        {
            _current = _actionQueue.Dequeue();
            _charAnimator.Animate(_current + "-before");
            RhythmTimer.OnTime.AddListener(CountSingleTurn);
            _turnCount++;
        }
        private void CountSingleTurn()
        {
            switch (_turnCount)
            {
                case 8:
                    _charAnimator.Animate(_current);
                    break;
                case 12:
                    EndAttack();
                    _data.StopAllAttacking();
                    RhythmTimer.OnTime.RemoveListener(CountSingleTurn);
                    break;
            }
            _turnCount++;
        }
        /// <summary>
        /// Combo - 3s attack, 1s damage. Also smooth end included.
        /// </summary>
        private void CountComboTurn()
        {
            switch (_turnCount)
            {
                case 1:
                    if (_actionQueue.Count != 0)
                    {
                        _current = _actionQueue.Dequeue();
                        _charAnimator.Animate(_current + "-before");
                        _data.StopAllAttacking();
                    }
                    else
                    {
                        _willAttackEnd = true;
                    }
                    break;
                case 3:
                    if (_willAttackEnd)
                    {
                        EndAttack();
                        _data.StopAllAttacking();
                        RhythmTimer.OnTime.RemoveListener(CountComboTurn);
                        _willAttackEnd = false;
                    }
                    break;
                case 7:
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
        private void EndAttack()
        {
            Attacking = false;
            OnAttackEnd.Invoke();
            OnAttackEnd.RemoveAllListeners();
        }
    }
}
