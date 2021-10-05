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
        /// Counts how many sequence it has
        /// </summary>
        private int _comboSequenceCount;
        /// <summary>
        /// Invoked, when the 'combo' is activated. Bool represents "May enter fever", and <c>true</c> if it has chance to enter fever
        /// </summary>
        public UnityEvent<(int comboCount, int sequenceCount)> OnCombo;
        /// <summary>
        /// Invoked, when the 'combo' is canceled.
        /// </summary>
        public UnityEvent OnComboCanceled;

        public RhythmFever FeverManager;
        private bool _hasFeverChance;
        private bool _isCombo;

        internal void CountCombo(RhythmCommandModel inputs)
        {
            if (RhythmFever.IsFever)
            {
                FeverManager.CheckFeverStatus(inputs);
                inputs.ComboType = ComboStatus.Fever;
                return;
            }

            _isCombo = true;
            _comboCount++;
            if (_comboCount > 9)
            {
                StartFever(inputs);
                return;
            }
            var perfectDrums = inputs.PerfectCount;
            if (perfectDrums == 0)
            {
                if (_hasFeverChance)
                {
                    _comboSequenceCount = 0;
                    _hasFeverChance = false;
                }
            }
            else
            {
                if (_comboCount > 1)
                {
                    if (_comboCount > 2 && perfectDrums == 4)
                    {
                        StartFever(inputs);
                        return;
                    }
                    _comboSequenceCount++;
                    if (_comboSequenceCount > 3)
                    {
                        StartFever(inputs);
                        return;
                    }
                    else
                    {
                        _hasFeverChance = true;
                    }
                }
            }
            inputs.ComboType = (_hasFeverChance) ? ComboStatus.MayFever : ComboStatus.NoFever;
            OnCombo.Invoke((_comboCount, _comboSequenceCount));
        }
        /// <summary>
        /// Ends combo in normal status, like miss drum hit. Invokes <see cref="OnComboCanceled"/>.
        /// </summary>
        internal void EndCombo()
        {
            if (!_isCombo) return;
            UnityEngine.Debug.Log("--------- Combo end! :( -----------");
            EndComboImmediately();
            OnComboCanceled.Invoke();
        }
        /// <summary>
        /// Ends combo due to specific reason e.g. miracle or mission success/fail. Doesn't invoke <see cref="OnComboCanceled"/>.
        /// </summary>
        internal void EndComboImmediately()
        {
            _isCombo = false;
            ClearCombo();
            FeverManager.EndFever();
        }
        internal void Destroy()
        {
            FeverManager.Destroy();
            OnCombo.RemoveAllListeners();
        }
        private void StartFever(RhythmCommandModel inputs)
        {
            inputs.ComboType = ComboStatus.Fever;
            ClearCombo();
            FeverManager.StartFever();
        }
        private void ClearCombo()
        {
            _comboCount = 0;
            _comboSequenceCount = 0;
            _hasFeverChance = false;
        }

        // PLEASE TELL US WHEN THE "MAY ENTER FEVER" CANCELED. EVEN WIKI DOESN'T TELL IT
        private bool IsBadDrum(RhythmCommandModel inputs) => inputs.BadCount > 1;
    }
}
