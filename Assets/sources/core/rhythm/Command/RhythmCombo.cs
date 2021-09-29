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
        /// Invoked, when the 'combo' is activated. Bool represents "May enter fever", and <c>true</c> if it has chance to enter fever
        /// </summary>
        public UnityEvent<bool> OnCombo;
        /// <summary>
        /// Invoked, when the 'combo' is canceled.
        /// </summary>
        public UnityEvent OnComboCanceled;

        public RhythmFever FeverManager;
        private bool _hasFeverChance;
        private bool _isCombo;

        internal void CountCombo(RhythmCommandModel inputs)
        {
            if (FeverManager.IsFever)
            {
                FeverManager.CheckFeverStatus(inputs);
                return;
            }

            _isCombo = true;
            UnityEngine.Debug.Log("~~ " + (_comboCount + 1) + " combo! ~~ ( O)( O)");
            _comboCount++;
            if (_comboCount > 9)
            {
                StartFever();
                return;
            }
            var perfectDrums = inputs.PerfectCount;
            if (perfectDrums == 0 && IsBadDrum(inputs))
            {
                if (_hasFeverChance)
                {
                    _perfectDrumCount = 0;
                    _hasFeverChance = false;
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
                        return;
                    }
                }
                else if (_comboCount > 1)
                {
                    _hasFeverChance = true;
                }
            }
            OnCombo.Invoke(_hasFeverChance);
        }
        internal void EndCombo()
        {
            if (!_isCombo) return;
            _isCombo = false;
            UnityEngine.Debug.Log("--------- Combo end! :( -----------");
            ClearCombo();
            OnComboCanceled.Invoke();
            FeverManager.EndFever();
        }
        internal void Destroy()
        {
            FeverManager.Destroy();
            OnCombo.RemoveAllListeners();
        }
        private void StartFever()
        {
            ClearCombo();
            FeverManager.StartFever();
        }
        private void ClearCombo()
        {
            _comboCount = 0;
            _perfectDrumCount = 0;
            _hasFeverChance = false;
        }

        // PLEASE TELL US WHEN THE "MAY ENTER FEVER" CANCELED. EVEN WIKI DOESN'T TELL IT
        private bool IsBadDrum(RhythmCommandModel inputs) => inputs.BadCount > 1;
    }
}
