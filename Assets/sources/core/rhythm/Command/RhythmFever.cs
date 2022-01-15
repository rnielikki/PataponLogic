using UnityEngine.Events;

namespace PataRoad.Core.Rhythm.Command
{
    /// <summary>
    /// Controls fever status.
    /// </summary>
    [System.Serializable]
    public class RhythmFever
    {
#pragma warning disable S1104 // Fields should not have public accessibility
        public static bool IsFever { get; private set; }
        public UnityEvent OnFever;
        public UnityEvent OnFeverWarning;
        public UnityEvent OnFeverCanceled;
#pragma warning restore S1104 // Fields should not have public accessibility - refactoring this will cause event listener reset on editor.

        private bool _isWarned;
        internal void StartFever()
        {
            IsFever = true;
            OnFever.Invoke();
        }
        /// <summary>
        /// Checks if you can keep the fever.
        /// </summary>
        /// <param name="inputs">The drum inputs, to check the drum hit status.</param>
        internal bool WillBeFeverStatus(RhythmCommandModel inputs)
        {
            if (!CanKeepFever(inputs))
            {
                if (!_isWarned)
                {
                    TurnCounter.OnNextTurn.AddListener(OnFeverWarning.Invoke);
                    _isWarned = true;
                }
                else
                {
                    TurnCounter.OnNextTurn.AddListener(EndFever);
                    return false;
                }
            }
            else _isWarned = false;
            return true;
        }

        internal void EndFever()
        {
            IsFever = false;
            _isWarned = false;
            OnFeverCanceled.Invoke();
        }
        internal void Destroy()
        {
            OnFever.RemoveAllListeners();
            OnFeverWarning.RemoveAllListeners();
            OnFeverCanceled.RemoveAllListeners();
        }
        private bool CanKeepFever(RhythmCommandModel inputs)
            => inputs.BadCount == 0 || inputs.PerfectCount > 0;
    }
}
