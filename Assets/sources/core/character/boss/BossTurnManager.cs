
using PataRoad.Core.Rhythm;
using PataRoad.Core.Rhythm.Command;
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
        private int _delay;
        internal BossTurnManager(BossAttackData data)
        {
            _data = data;
            _charAnimator = data.CharAnimator;
        }
        //-- start and end
        /// <summary>
        /// Starts the boss attack from queue. This ends when queue ends.
        /// </summary>
        public void StartAttack(int turnDelay)
        {
            if (Attacking) return;
            Attacking = true;
            _turnCount = 0;
            if (turnDelay == 0)
            {
                Launch();
            }
            else
            {
                _delay = turnDelay * 4 - 1;
                RhythmTimer.Current.OnTime.AddListener(WaitForDelay);
            }
        }
        public void End(bool stopAllAttacking = true)
        {
            _actionQueue.Clear();
            if (RhythmTimer.Current != null)
            {
                RhythmTimer.Current.OnTime.RemoveListener(WaitForDelay);
                RhythmTimer.Current.OnTime.RemoveListener(CountSingleTurn);
                RhythmTimer.Current.OnTime.RemoveListener(CountComboTurn);
            }
            Rhythm.Command.TurnCounter.OnNonPlayerTurn.RemoveListener(CountSingleAttack);
            if (stopAllAttacking) _data.StopAllAttacking();
            Attacking = false;
        }
        // -- normal actions
        public void SetOneAction(string actionName)
        {
            if (Attacking) return;
            _actionQueue.Enqueue(actionName);
        }
        /// <summary>
        /// Adds action, regardless of what action was performed last time.
        /// </summary>
        /// <param name="actionName"></param>
        public void DefineNextAction(string actionName)
        {
            End();
            _actionQueue.Enqueue(actionName);
            StartAttack(0);
        }
        //-- combo
        public void SetComboAttack(IEnumerable<string> actions)
        {
            if (Attacking) return;
            foreach (var action in actions) _actionQueue.Enqueue(action);
        }
        private void WillStartComboAttack()
        {
            RhythmTimer.Current.OnNext.AddListener(() => RhythmTimer.Current.OnTime.AddListener(CountComboTurn));
        }

        private void CountSingleAttack()
        {
            _current = _actionQueue.Dequeue();
            if (_current != "nothing" && _current != "Idle") _charAnimator.Animate(_current + "-before");
            RhythmTimer.Current.OnTime.AddListener(CountSingleTurn);
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
                    RhythmTimer.Current.OnTime.RemoveListener(CountSingleTurn);
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
                        if (_current != "nothing" && _current != "Idle") _charAnimator.Animate(_current + "-before");
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
                        RhythmTimer.Current.OnTime.RemoveListener(CountComboTurn);
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
        private void WaitForDelay()
        {
            if (_delay == 0)
            {
                Launch();
                RhythmTimer.Current.OnTime.RemoveListener(WaitForDelay);
            }
            _delay--;
        }
        private void Launch()
        {
            if (_actionQueue.Count > 1)
            {
                TurnCounter.OnNonPlayerTurn.AddListener(WillStartComboAttack);
            }
            else
            {
                TurnCounter.OnNonPlayerTurn.AddListener(CountSingleAttack);
            }
        }
    }
}
