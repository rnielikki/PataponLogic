using System.Collections.Generic;
using System.Linq;

namespace PataRoad.Core.Rhythm.Command
{
    public class PracticingMiracleListener : MiracleListener
    {
        private MiracleListener _original;
        private RhythmCommand _command;
        public int PracticingCount { get; private set; }
        public const int FullPracticingCount = 1;

        public UnityEngine.Events.UnityEvent<int> OnMiracleDrumHit { get; } = new UnityEngine.Events.UnityEvent<int>();
        public UnityEngine.Events.UnityEvent OnMiracleDrumMiss { get; } = new UnityEngine.Events.UnityEvent();
        public UnityEngine.Events.UnityEvent OnMiraclePracticingEnd { get; } = new UnityEngine.Events.UnityEvent();

        private void Awake()
        {
            OnMiracle = new UnityEngine.Events.UnityEvent();
        }
        internal PracticingMiracleListener LoadFromMiracleListener(RhythmCommand sender, MiracleListener listener)
        {
            _command = sender;
            _original = listener;
            listener.enabled = false;
            listener.SetCurrentMiracleDrumTo(this);
            OnMiracle.AddListener(CountMiracle);
            _command.OnCommandCanceled.AddListener(ResetMiracleCount);
            OnMiracle.AddListener(() => FindObjectOfType<Bgm.RhythmBgmSinging>().SingMiracle());
            return this;
        }
        internal override bool HasMiracleChance(IEnumerable<DrumType> currentDrums, RhythmInputModel input)
        {
            if (!TurnCounter.IsOn) return false;
            var hasMiracleChance = CheckMiracleChance(currentDrums, input);
            if (hasMiracleChance)
            {
                OnMiracleDrumHit.Invoke(currentDrums.Count());
            }
            return hasMiracleChance;
        }
        private void CountMiracle()
        {
            ResetMiracle();
            PracticingCount++;
            if (PracticingCount > FullPracticingCount)
            {
                OnMiraclePracticingEnd.Invoke();
                CleanListeners();
                StopMiraclePracticing();
            }
        }
        private void StopMiraclePracticing()
        {
            _command.SetMiracleListener(_original);
            enabled = false;
            ResetMiracle();
        }
        private void ResetMiracleCount()
        {
            ResetMiracle();
            OnMiracleDrumMiss.Invoke();
        }
        private void CleanListeners()
        {
            OnMiracleDrumHit.RemoveAllListeners();
            OnMiracleDrumMiss.RemoveAllListeners();
            OnMiracle.RemoveAllListeners();
            _command.OnCommandCanceled.RemoveListener(ResetMiracleCount);
            OnMiraclePracticingEnd.RemoveAllListeners();
        }
        private void OnDisable()
        {
            CleanListeners();
        }
    }
}
