using UnityEngine.Events;

namespace Core.Rhythm.Command
{
    /// <summary>
    /// Counts turn. Separated from timer, because "turn" can be activated or deactivated, depends on the input/command/combo state.
    /// Also, the "turn" works globally in the game, so it's static
    /// </summary>
    public static class TurnCounter
    {
        private static int _count;
        /// <summary>
        /// Called ONCE when reaches the next turn
        /// </summary>
        public static UnityEvent OnNextTurn { get; } = new UnityEvent();
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
            IsOn = true;
            IsPlayerTurn = false;
            RhythmTimer.OnTime.AddListener(Count);
        }
        internal static void Stop()
        {
            IsOn = false;
            IsPlayerTurn = true;
            RhythmTimer.OnTime.RemoveListener(Count);
            OnNextTurn.RemoveAllListeners();
            _count = 0;
        }
        private static void Count()
        {
            switch (_count)
            {
                case 0:
                    OnTurn.Invoke();
                    OnNextTurn.Invoke();
                    OnNextTurn.RemoveAllListeners();
                    break;
                case 3:
                    RhythmTimer.OnNextHalfTime.AddListener(() => IsPlayerTurn = !IsPlayerTurn);
                    break;
            }
            _count = (_count + 1) % 4;
        }
    }
}
