namespace Core.Rhythm.Model
{
    /// <summary>
    /// Model for sending event in Rhythm drum hit event.
    /// </summary>
    public class RhythmInputModel
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
        /// If <see cref="RhythmTimer.Count"/> was bigger than <see cref="RhythmTimer.HalfFrequency"/> when pressed, this is <code>true</code>.
        /// <note>The primary purpose is checking when is the 'next timing after command', but can be used somewhere else</note>
        /// </summary>
        public bool IfLater { get; }
        /// <summary>
        /// The timing for the drum; This is kept, especially for DON miracle hit
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
            Timing = RhythmTimer.GetTiming(count);
            Status = GetHitStatus(Timing);
            if (Status != DrumHitStatus.Miss)
            {
                IfLater = (count < RhythmTimer.HalfFrequency && count != 0);
            }
        }
        /// <summary>
        /// For returning "Miss" status from <see cref="Miss(DrumType)"/> method.
        /// </summary>
        /// <param name="drum"></param>
        private RhythmInputModel(DrumType drum)
        {
            Drum = drum;
            Timing = RhythmTimer.Count;
            Status = DrumHitStatus.Miss;
        }
        /// <summary>
        /// Get "Miss" Status of the drum, regardless of timing.
        /// </summary>
        /// <param name="drum">The <see cref="DrumType"/> that represents Miss status.</param>
        /// <returns>Miss hit status of the drum.</returns>
        internal static RhythmInputModel Miss(DrumType drum) => new RhythmInputModel(drum);
        private DrumHitStatus GetHitStatus(int timing)
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
}
