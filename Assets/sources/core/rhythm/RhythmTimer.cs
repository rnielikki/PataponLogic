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

        //Events
        /// <summary>
        /// When the timer service (the whole game with Rhythm) is actually started.
        /// </summary>
        public static UnityEvent OnStart { get; } = new UnityEvent();
        /// <summary>
        /// Event, when rhythm reaches in just right time.
        /// </summary>
        public static UnityEvent OnTime { get; } = new UnityEvent();
        /// <summary>
        /// Event, when rhythm reaches in half of right time (half of frequency).
        /// </summary>
        public static UnityEvent OnHalfTime { get; } = new UnityEvent();
        /// <summary>
        /// This is called ONLY ONCE when reaches in right time (0 frequency count).
        /// </summary>
        public static UnityEvent OnNext { get; } = new UnityEvent();
        /// <summary>
        /// This is called ONLY ONCE when reaches in half of right time (half of frequency).
        /// </summary>
        public static UnityEvent OnNextHalfTime { get; } = new UnityEvent();

        private void Awake()
        {
            Frequency = (int)(RhythmEnvironment.InputInterval / Time.fixedDeltaTime);
            HalfFrequency = Frequency / 2;
            PerfectFrequency = (int)(RhythmEnvironment.PerfectRange / Time.fixedDeltaTime);
            GoodFrequency = (int)(RhythmEnvironment.GoodRange / Time.fixedDeltaTime);
            BadFrequency = (int)(RhythmEnvironment.BadRange / Time.fixedDeltaTime);
        }
        private void Start()
        {
            //A bit tricky way to start
            OnNextHalfTime.AddListener(() =>
            {
                OnNext.AddListener(OnStart.Invoke);
            });
        }

        private void FixedUpdate()
        {
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
            //Note: If you got DividedByZero Exception in here on editor, just DON'T EDIT SCRIPT WHILE EXECUTING.
            //It's very common to throw exception when script is modified on execution.
            Count = (Count + 1) % Frequency;
        }

        private void OnDestroy()
        {
            StopAndRemoveAllListeners();
        }

        public static int GetTiming() => GetTiming(Count);
        internal static int GetTiming(int count)
        {
            if (count < HalfFrequency) return count;
            else return Frequency - count;
        }
        internal static int GetHalfTiming(int count) => Mathf.Abs(HalfFrequency - count);
        public void StopAndRemoveAllListeners()
        {
            Command.TurnCounter.Stop();
            OnTime.RemoveAllListeners();
            OnHalfTime.RemoveAllListeners();
            OnNext.RemoveAllListeners();
            OnNextHalfTime.RemoveAllListeners();
        }
    }
}
