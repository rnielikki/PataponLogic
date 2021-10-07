using UnityEngine;

namespace Core.Rhythm
{
    /// <summary>
    /// Wait for specific time frequency (<see cref="RhythmTimer.Count"/>) in coroutine.
    /// <note>This checks time AFTER <see cref="RhythmTimer"/> is updated.</note>
    /// </summary>
    public class WaitForRhythmTime : CustomYieldInstruction
    {
        private readonly int _waitingTime;
        public override bool keepWaiting
        {
            get => _waitingTime != RhythmTimer.Count;
        }
        public WaitForRhythmTime(int waitingTime)
        {
            _waitingTime = waitingTime;
        }
    }
}
