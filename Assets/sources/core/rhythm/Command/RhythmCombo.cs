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
        /// Counts the sum of the perfect drum. Counts from very first and doesn't reset if combo doesn't end
        /// </summary>
        private int _perfectDrumSum = 0;
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
                    _perfectDrumSum = inputs.PerfectCount;
                    return ComboStatus.NoFever;
                }
            }

            _isCombo = true;
            _comboCount++;
            _perfectDrumSum += inputs.PerfectCount;
            if (_comboCount >= RhythmEnvironment.FeverMaximum)
            {
                StartFever();
                return ComboStatus.Fever;
            }
            if (inputs.BadCount > 0)
            {
                if (_hasFeverChance)
                {
                    _hasFeverChance = false;
                }
            }
            else if (_comboCount > 2)
            {
                if ((inputs.PerfectCount == 4 && RhythmEnvironment.Difficulty == Difficulty.Easy)
                    || (_perfectDrumSum >= 10 && LoadPerfectRequirement() <= inputs.PerfectCount))
                {
                    StartFever();
                    return ComboStatus.Fever;
                }
                else
                {
                    _hasFeverChance = true;
                }
            }
            if (!_hasFeverChance) _perfectDrumSum = 0;
            TurnCounter.OnNextTurn.AddListener(() => OnCombo.Invoke(new RhythmComboModel(inputs, _comboCount)));
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
            OnComboCanceled.RemoveAllListeners();
        }
        private int LoadPerfectRequirement() =>
        _comboCount switch
        {
            3 => 4,
            4 => 3,
            _ => 2
        };
        private void StartFever()
        {
            ClearCombo();
            TurnCounter.OnNextTurn.AddListener(FeverManager.StartFever);
        }
        private void ClearCombo()
        {
            _comboCount = 0;
            _perfectDrumSum = 0;
            _hasFeverChance = false;
        }
    }
}
