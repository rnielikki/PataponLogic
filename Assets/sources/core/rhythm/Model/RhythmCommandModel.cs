using System.Collections.Generic;
using System.Linq;

namespace Core.Rhythm.Command
{
    public class RhythmCommandModel
    {
        /// <summary>
        /// The 4-beat drum command., e.g. PATA PON DON CHAKA
        /// </summary>
        public CommandSong Song { get; }

        /// <summary>
        /// Represents if the command is on fever, or may enter fever.
        /// </summary>
        public ComboStatus ComboType { get; internal set; }

        /// <summary>
        /// How many drum hit was perfect inside the command.
        /// </summary>
        public int PerfectCount { get; }
        /// <summary>
        /// How many drum hit was bad inside the command.
        /// </summary>
        public int BadCount { get; }

        /// <summary>
        /// The accuracy percentage. Will affect min-max damage/defence and more.
        /// </summary>
        public float Percentage { get; }

        internal RhythmCommandModel(IEnumerable<RhythmInputModel> inputs, CommandSong song)
        {
            Song = song;
            var statusCollection = inputs.Select(hit => hit.Status);
            PerfectCount = statusCollection.Count(status => status == DrumHitStatus.Perfect);
            BadCount = statusCollection.Count(status => status == DrumHitStatus.Bad);
            Percentage = GetPercentage(inputs.Sum(input => input.Timing));
        }
        private float GetPercentage(int timingSum) => 1 - UnityEngine.Mathf.InverseLerp(RhythmTimer.PerfectFrequency * 4, RhythmTimer.GoodFrequency * 4, timingSum);
    }
}
