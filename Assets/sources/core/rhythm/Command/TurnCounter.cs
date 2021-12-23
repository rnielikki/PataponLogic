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
        /// Called ONCE when reaches the next turn AND PLAYER turn.
        /// </summary>
        public static UnityEvent OnPlayerTurn { get; } = new UnityEvent();

        /// <summary>
        /// Called ONCE when reaches the next turn AND NON-PLAYER turn.
        /// </summary>
        public static UnityEvent OnNonPlayerTurn { get; } = new UnityEvent();
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
            RhythmTimer.Current.OnTime.AddListener(Count);
        }
        internal static void Stop()
        {
            if (IsOn)
            {
                OnNextTurn.RemoveAllListeners();
            }
            IsOn = false;
            IsPlayerTurn = true;
            RhythmTimer.Current?.OnTime?.RemoveListener(Count);
        }
        internal static void Destroy()
        {
            Stop();
            OnPlayerTurn.RemoveAllListeners();
            OnNonPlayerTurn.RemoveAllListeners();
            OnTurn.RemoveAllListeners();
        }
        private static void Count()
        {
            switch (TurnCount)
            {
                case 0:
                    OnTurn.Invoke();
                    OnNextTurn.Invoke();
                    OnNextTurn.RemoveAllListeners();
                    if (IsPlayerTurn)
                    {
                        OnPlayerTurn.Invoke();
                        OnPlayerTurn.RemoveAllListeners();
                    }
                    else
                    {
                        OnNonPlayerTurn.Invoke();
                        OnNonPlayerTurn.RemoveAllListeners();
                    }
                    break;
                case 3:
                    RhythmTimer.Current.OnNextHalfTime.AddListener(() => IsPlayerTurn = !IsPlayerTurn);
                    break;
            }
            TurnCount = (TurnCount + 1) % 4;
        }
    }
}
