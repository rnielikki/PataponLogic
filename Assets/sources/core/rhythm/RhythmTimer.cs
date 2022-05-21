using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Rhythm
{
    /// <summary>
    /// The core timer, using "frequency" system.
    /// <remarks>This always starts (and should it be) priorior to other scripts.</remarks>
    /// </summary>
    public class RhythmTimer : MonoBehaviour
    {
        //Frequency System
        /// <summary>
        /// How much far away from Frequency. Range is from 0 to (frequency-1).
        /// </summary>
        public static int Count { get; private set; }
        /// <summary>
        /// How frequent the application reads the value. Maximum value of <see cref="Count"/>. The smaller <see cref="Time.fixedDeltaTime"/> is, the value is bigger.
        /// </summary>
        public static int Frequency { get; private set; }
        /// <summary>
        /// Half of the <see cref="Frequency"/> value.
        /// </summary>
        public static int HalfFrequency { get; private set; }
        /// <summary>
        /// How much <see cref="Count"/> is considered as perfect hit.
        /// </summary>
        public static int PerfectFrequency { get; private set; }
        /// <summary>
        /// How much <see cref="Count"/> is considered as good hit.
        /// </summary>
        public static int GoodFrequency { get; private set; }
        /// <summary>
        /// How much <see cref="Count"/> is considered as bad hit.
        /// </summary>
        public static int BadFrequency { get; private set; }
        /// <summary>
        /// Threshold of minimal effect <see cref="RhythmEnvironment.MinEffectThreshold"/>, as frequency, as <see cref="Count"/>.
        /// </summary>
        public static int MinEffectThresholdFrequency { get; private set; }
        /// <summary>
        /// 1/4 of the <see cref="Frequency"/> value.
        /// </summary>
        public static int QuarterFrequency { get; private set; }

        [SerializeField]
        private UnityEvent _onStart;
        //Events
        /// <summary>
        /// When the timer service (the whole game with Rhythm) is actually started.
        /// </summary>
        public UnityEvent OnStart { get; private set; }

        [SerializeField]
        private UnityEvent _onTime;
        /// <summary>
        /// Event, when rhythm reaches in just right time.
        /// </summary>
        public UnityEvent OnTime { get; private set; }

        [SerializeField]
        private UnityEvent _onHalftime;
        /// <summary>
        /// Event, when rhythm reaches in half of right time (half of frequency).
        /// </summary>
        public UnityEvent OnHalfTime { get; private set; }

        [SerializeField]
        private UnityEvent _onNextQuarterTime;
        /// <summary>
        /// Event, which is called in next quarter time for miracle.
        /// </summary>
        public UnityEvent OnNextQuarterTime { get; private set; }

        [SerializeField]
        private UnityEvent _onNext;
        /// <summary>
        /// This is called ONLY ONCE when reaches in right time (0 frequency count).
        /// </summary>
        public UnityEvent OnNext { get; private set; }
        /// <summary>
        /// This is called ONLY ONCE when reaches in half of right time (half of frequency).
        /// </summary>
        public UnityEvent OnNextHalfTime { get; } = new UnityEvent();

        [SerializeField]
        private bool UseHalfOfTime;

        /// <summary>
        /// Offset as frequency, to keep drum in sync to music. Used for drum calculation.
        /// </summary>
        /// <note>This applies to all musics. Since it affects to every music, may need to change music source without touching this.</note>
        public const int FrequencyOffset = 0; //offset to sync w music.

        public static RhythmTimer Current { get; private set; }

        private void Awake()
        {
            Current = this;
            OnStart = _onStart;
            OnTime = _onTime;
            OnHalfTime = _onHalftime;
            OnNextQuarterTime = _onNextQuarterTime;
            OnNext = _onNext;

            UnityEngine.InputSystem.InputSystem.settings.updateMode
                = UnityEngine.InputSystem.InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
        }
        private void Start()
        {
            StartTimer();
        }
        public void StartTimer()
        {
            Frequency = (int)(RhythmEnvironment.InputInterval / Time.fixedDeltaTime);
            HalfFrequency = Frequency / 2;
            QuarterFrequency = Frequency / 4;
            PerfectFrequency = (int)(RhythmEnvironment.PerfectRange / Time.fixedDeltaTime);
            GoodFrequency = (int)(RhythmEnvironment.GoodRange / Time.fixedDeltaTime);
            BadFrequency = (int)(RhythmEnvironment.BadRange / Time.fixedDeltaTime);
            MinEffectThresholdFrequency = (int)(RhythmEnvironment.MinEffectThreshold / Time.fixedDeltaTime);
            if (UseHalfOfTime)
            {
                Frequency /= 2;
                HalfFrequency /= 2;
                QuarterFrequency /= 2;
                PerfectFrequency /= 2;
                GoodFrequency /= 2;
                BadFrequency /= 2;
                MinEffectThresholdFrequency /= 2;
            }
            Count = 0;
            OnNextHalfTime.AddListener(() => OnNext.AddListener(OnStart.Invoke));
        }

        private void FixedUpdate()
        {
            if (!Application.isFocused) return;
            if (Count == 0)
            {
                OnTime.Invoke();
                OnNext.Invoke();
                OnNext.RemoveAllListeners();
            }
            else if (Count == HalfFrequency - 1)
            {
                OnHalfTime.Invoke();
                OnNextHalfTime.Invoke();
                OnNextHalfTime.RemoveAllListeners();
            }
            else if (Count == QuarterFrequency - 1 || Count == 3 * QuarterFrequency - 1)
            {
                OnNextQuarterTime.Invoke();
                OnNextQuarterTime.RemoveAllListeners();
            }
            //Note: If you got DividedByZero Exception in here on editor, just DON'T EDIT SCRIPT WHILE EXECUTING.
            //It's very common to throw exception when script is modified on execution.
            Count = (Count + 1) % Frequency;
        }

        private void OnDestroy()
        {
            StopAndRemoveAllListeners();
            Current = null;
        }
        /// <summary>
        /// Returns timing, which means smaller is better hit. This calculates automatically rhythm offset, if offset is set.
        /// </summary>
        /// <returns>Timing value, that can be used normally.</returns>
        public static int GetTiming() => GetTiming(GetDrumCount());
        /// <summary>
        /// Returns timing without absolute value. - Means earlier, + means later. This calculates automatically rhythm offset, if offset is set.
        /// </summary>
        /// <returns>Timing offset to the frequency, which can be plus or minus.</returns>
        public static int GetTimingWithDirection(int count)
        {
            if (count < HalfFrequency) return count;
            else return count - Frequency;
        }
        internal static int GetTiming(int count)
        {
            if (count < HalfFrequency) return count;
            else return Frequency - count;
        }
        /// <summary>
        /// Gets drum frequency with offset. It helps to make sync with music. DON'T USE THIS FOR GETTING TIMING, use <see cref="GetTiming"/> instead.
        /// </summary>
        /// <returns>Offset drum value which is sync with music.</returns>
        /// <note>If it's still not in sync with music, change <see cref="FrequencyOffset"/> or music soruce.</note>
        public static int GetDrumCount()
        {
            return (Count - FrequencyOffset + Frequency) % Frequency;
        }
        public void StopAndRemoveAllListeners()
        {
            Command.TurnCounter.Destroy();
            OnTime.RemoveAllListeners();
            OnHalfTime.RemoveAllListeners();
            OnNext.RemoveAllListeners();
            OnNextHalfTime.RemoveAllListeners();
            OnNextQuarterTime.RemoveAllListeners();

            UnityEngine.InputSystem.InputSystem.settings.updateMode
                = UnityEngine.InputSystem.InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
        }
    }
}
