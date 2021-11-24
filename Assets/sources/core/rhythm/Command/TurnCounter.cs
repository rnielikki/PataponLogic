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
        /// Called ONCE when reaches the next turn AND BOSS turn. It's half of non-player turn.
        /// </summary>
        public static UnityEvent OnNextBossTurn { get; } = new UnityEvent();
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
            }
            IsOn = false;
            IsPlayerTurn = true;
            RhythmTimer.OnTime.RemoveListener(Count);
        }
        internal static void Destroy()
        {
            Stop();
            OnNextBossTurn.RemoveAllListeners();
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
                    break;
                case 2:
                    if (!IsPlayerTurn)
                    {
                        OnNextBossTurn.Invoke();
                        OnNextBossTurn.RemoveAllListeners();
                    }
                    break;
                case 3:
                    RhythmTimer.OnNextHalfTime.AddListener(() => IsPlayerTurn = !IsPlayerTurn);
                    break;
            }
            TurnCount = (TurnCount + 1) % 4;
        }
    }
}
