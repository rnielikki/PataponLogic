using UnityEngine.Events;

namespace PataRoad.Core.Rhythm.Command
{
    /// <summary>
    /// Counts turn. Separated from timer, because "turn" can be activated or deactivated, depends on the input/command/combo state.
    /// Also, the "turn" works globally in the game, so it's static
    /// </summary>
    public static class TurnCounter
    {
        public static int TurnCount { get; private set; }
        /// <summary>
        /// Called ONCE when reaches the next turn
        /// </summary>
        public static UnityEvent OnNextTurn { get; } = new UnityEvent();
        /// <summary>
        /// Called ONCE when reaches the next turn AND player turn
        /// </summary>
        public static UnityEvent OnNextPlayerTurn { get; } = new UnityEvent();
        /// <summary>
        /// Called ONCE when reaches the next turn AND NOT player turn
        /// </summary>
        public static UnityEvent OnNextNonPlayerTurn { get; } = new UnityEvent();
        /// <summary>
        /// Called EVERY TIME when reaches the next turn
        /// </summary>
        public static UnityEvent OnTurn { get; } = new UnityEvent();
        /// <summary>
        /// If the turn is counting.
        /// </summary>
        public static bool IsOn { get; private set; }
        /// <summary>
        /// Check if the turn is for player
        /// </summary>
        public static bool IsPlayerTurn { get; private set; } = true;
        internal static void Start()
        {
            TurnCount = 0;
            IsOn = true;
            IsPlayerTurn = false;
            RhythmTimer.OnTime.AddListener(Count);
        }
        internal static void Stop()
        {
            if (IsOn)
            {
                OnNextTurn.RemoveAllListeners();
                OnNextPlayerTurn.RemoveAllListeners();
                OnNextNonPlayerTurn.RemoveAllListeners();
            }
            IsOn = false;
            IsPlayerTurn = true;
            RhythmTimer.OnTime.RemoveListener(Count);
        }
        internal static void Destroy()
        {
            Stop();
            OnTurn.RemoveAllListeners();
        }
        private static void Count()
        {
            switch (TurnCount)
            {
                case 0:
                    OnTurn.Invoke();
                    OnNextTurn.Invoke();
                    if (IsPlayerTurn)
                    {
                        OnNextPlayerTurn.Invoke();
                        OnNextPlayerTurn.RemoveAllListeners();
                    }
                    else
                    {
                        OnNextNonPlayerTurn.Invoke();
                        OnNextNonPlayerTurn.RemoveAllListeners();
                    }
                    OnNextTurn.RemoveAllListeners();
                    break;
                case 3:
                    RhythmTimer.OnNextHalfTime.AddListener(() => IsPlayerTurn = !IsPlayerTurn);
                    break;
            }
            TurnCount = (TurnCount + 1) % 4;
        }
    }
}
