using Core.Rhythm.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace Core.Rhythm.Command
{
    [System.Serializable]
    internal class RhythmCombo
    {
        /// <summary>
        /// Counts how many combo currently is.
        /// </summary>
        private int _comboCount;
        /// <summary>
        /// Counts how many perfect drum in this combo
        /// </summary>
        private int _perfectDrumCount;
        /// <summary>
        /// Invoked, when the first 'combo' is activated.
        /// </summary>
        public UnityEvent OnFirstCombo;
        /// <summary>
        /// Invoked, when the combo has chance to enter fever. (Sound changes, combo worm bounces...)
        /// </summary>
        public UnityEvent OnChanceFever;
        /// <summary>
        /// Invoked, when the chance to fever has been canceled (e.g. some drums may go bad...)
        /// </summary>
        public UnityEvent OnFeverChanceCanceled;

        public RhythmFever FeverManager;
        private bool _hasFeverChance;

        internal void CountCombo(RhythmCommandModel inputs)
        {
            if (FeverManager.IsFever)
            {
                FeverManager.CheckFeverStatus(inputs);
                return;
            }

            UnityEngine.Debug.Log("~~ " + _comboCount + " combo! ~~ ( O)( O)");
            //first action
            if (_comboCount == 0)
            {
                OnFirstCombo.Invoke();
            }
            _comboCount++;
            if (_comboCount > 9)
            {
                StartFever();
            }
            var perfectDrums = inputs.PerfectCount;
            if (perfectDrums == 0 && IsBadDrum(inputs))
            {
                if (_hasFeverChance)
                {
                    _perfectDrumCount = 0;
                    _hasFeverChance = false;
                    OnFeverChanceCanceled.Invoke(); //"cancel" invoke should called immediately
                }
            }
            else
            {
                if (_comboCount > 2)
                {
                    _perfectDrumCount += perfectDrums;
                    if (_perfectDrumCount > 3)
                    {
                        StartFever();
                    }
                }
                else if (_comboCount > 1)
                {
                    _hasFeverChance = true;
                    RhythmTimer.InvokeNext(OnChanceFever);
                }
            }
        }
        internal void EndCombo()
        {
            UnityEngine.Debug.Log("--------- Combo end! :( -----------");
            _comboCount = 0;
            _perfectDrumCount = 0;
            _hasFeverChance = false;
            OnFeverChanceCanceled.Invoke(); //"cancel" invoke should called immediately
            FeverManager.EndFever();
        }
        internal void Destroy()
        {
            FeverManager.Destroy();
            OnFirstCombo.RemoveAllListeners();
            OnChanceFever.RemoveAllListeners();
            OnFeverChanceCanceled.RemoveAllListeners();
        }
        private void StartFever()
        {
            FeverManager.StartFever();
            _comboCount = 0;
            _perfectDrumCount = 0;
        }
        // PLEASE TELL US WHEN THE "MAY ENTER FEVER" CANCELED. EVEN WIKI DOESN'T TELL IT
        private bool IsBadDrum(RhythmCommandModel inputs) => inputs.BadCount > 1;
    }
}
