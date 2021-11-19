using UnityEngine.Events;

namespace PataRoad.Core.Rhythm.Command
{
    /// <summary>
    /// Defines combo status.
    /// </summary>
    [System.Serializable]
    public class RhythmCombo
    {
#pragma warning disable S1104 // Fields should not have public accessibility
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
        /// <note>This is separated from <see cref="RhythmCombo.OnCombo"/> - FEVER status WON'T call this event.</note>
        /// </summary>
        [UnityEngine.Header("Note: FEVER status WON'T call this event")]
        public UnityEvent<RhythmComboModel> OnCombo;
        /// <summary>
        /// Invoked, when the 'combo' is canceled.
        /// </summary>
        public UnityEvent OnComboCanceled;

        public RhythmFever FeverManager;
        private bool _hasFeverChance;
        private bool _isCombo;

#pragma warning restore S1104 // Fields should not have public accessibility - refactoring this will cause event listener reset on editor.
        internal ComboStatus CountCombo(RhythmCommandModel inputs)
        {
            if (RhythmFever.IsFever)
            {
                if (FeverManager.WillBeFeverStatus(inputs))
                {
                    return ComboStatus.Fever;
                }
                else
                {
                    return ComboStatus.NoFever;
                }
            }

            _isCombo = true;
            _comboCount++;
            if (_comboCount > 9)
            {
                StartFever();
                return ComboStatus.Fever;
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
                        StartFever();
                        return ComboStatus.Fever;
                    }
                    _comboSequenceCount++;
                    if (_comboSequenceCount > 3)
                    {
                        StartFever();
                        return ComboStatus.Fever;
                    }
                    else
                    {
                        _hasFeverChance = true;
                    }
                }
            }
            TurnCounter.OnNextTurn.AddListener(() => OnCombo.Invoke(new RhythmComboModel(inputs, _comboCount, _comboSequenceCount)));
            return (_hasFeverChance) ? ComboStatus.MayFever : ComboStatus.NoFever;
        }
        /// <summary>
        /// Ends combo in normal status, like miss drum hit. Invokes <see cref="OnComboCanceled"/>.
        /// </summary>
        internal void EndCombo()
        {
            if (!_isCombo) return;
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
        private void StartFever()
        {
            ClearCombo();
            TurnCounter.OnNextTurn.AddListener(FeverManager.StartFever);
        }
        private void ClearCombo()
        {
            _comboCount = 0;
            _comboSequenceCount = 0;
            _hasFeverChance = false;
        }
    }
}
