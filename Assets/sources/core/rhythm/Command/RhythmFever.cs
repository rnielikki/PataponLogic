﻿using UnityEngine.Events;

namespace Core.Rhythm.Command
{
    /// <summary>
    /// Controls fever status.
    /// </summary>
    [System.Serializable]
    class RhythmFever
    {
        public static bool IsFever { get; private set; }
        public UnityEvent OnFever;
        public UnityEvent OnFeverWarning;
        public UnityEvent OnFeverCanceled;

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
                    OnFeverWarning.Invoke();
                    _isWarned = true;
                }
                else
                {
                    EndFever();
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
        //What's the condition of "keeping fever"? I don't really know... you can fix the logic if you know better
        private bool CanKeepFever(RhythmCommandModel inputs) =>
             inputs.BadCount < 3;
    }
}
