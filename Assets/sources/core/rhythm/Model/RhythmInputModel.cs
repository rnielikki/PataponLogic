namespace PataRoad.Core.Rhythm
{
    /// <summary>
    /// Model for sending event in Rhythm drum hit event.
    /// </summary>
    public struct RhythmInputModel
    {
        /// <summary>
        /// The <see cref="DrumHitStatus"/>, which represents that if it's perfect, good, bad or miss.
        /// </summary>
        public DrumHitStatus Status { get; }
        /// <summary>
        /// The drum that pressed.
        /// </summary>
        public DrumType Drum { get; }
        /// <summary>
        /// Count value when the drum was hit. This kept for DON miracle hit.
        /// </summary>
        public int Count { get; }
        /// <summary>
        /// The timing for the drum, as frequency.
        /// </summary>
        public int Timing { get; }
        /// <summary>
        /// Generates model for sending rhythm drum hit event.
        /// </summary>
        /// <param name="drum">The drum tpye.</param>
        /// <param name="count">The <see cref="RhythmTimer.Count"/> value when pressed.</param>
        internal RhythmInputModel(DrumType drum, int count)
        {
            Drum = drum;
            Count = count;
            Timing = RhythmTimer.GetTiming(count);
            Status = GetHitStatus(Timing);
            DrumHitStatus GetHitStatus(int timing)
            {
                if (timing <= RhythmTimer.PerfectFrequency)
                {
                    return DrumHitStatus.Perfect;
                }
                else if (timing <= RhythmTimer.GoodFrequency)
                {
                    return DrumHitStatus.Good;
                }
                else if (timing <= RhythmTimer.BadFrequency)
                {
                    return DrumHitStatus.Bad;
                }
                else
                {
                    return DrumHitStatus.Miss;
                }
            }
        }
        /// <summary>
        /// Generates model with defined drum status.
        /// </summary>
        /// <param name="drum">The drum tpye.</param>
        /// <param name="count">The <see cref="RhythmTimer.Count"/> value when pressed.</param>
        internal RhythmInputModel(DrumType drum, int count, DrumHitStatus status)
        {
            Drum = drum;
            Count = count;
            Timing = RhythmTimer.GetTiming(count);
            Status = status;
        }

        /// <summary>
        /// For returning "Miss" status from <see cref="Miss(DrumType)"/> method.
        /// </summary>
        /// <param name="drum"></param>
        private RhythmInputModel(DrumType drum)
        {
            Drum = drum;
            Count = 0;
            Timing = 0;
            Status = DrumHitStatus.Miss;
        }
        /// <summary>
        /// Get "Miss" Status of the drum, regardless of timing.
        /// </summary>
        /// <param name="drum">The <see cref="DrumType"/> that represents Miss status.</param>
        /// <returns>Miss hit status of the drum.</returns>
        internal static RhythmInputModel Miss(DrumType drum) => new RhythmInputModel(drum);
    }
}
