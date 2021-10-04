using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Rhythm.Model
{
    public class RhythmCommandModel
    {
        /// <summary>
        /// The 4-beat drum command., e.g. PATA PON DON CHAKA
        /// </summary>
        public CommandSong Song { get; }

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
        private static float _minMax = (RhythmTimer.BadFrequency - RhythmTimer.PerfectFrequency) * 4;

        internal RhythmCommandModel(IEnumerable<RhythmInputModel> inputs, CommandSong song)
        {
            Song = song;
            var statusCollection = inputs.Select(hit => hit.Status);
            PerfectCount = statusCollection.Count(status => status == DrumHitStatus.Perfect);
            BadCount = statusCollection.Count(status => status == DrumHitStatus.Bad);
            Percentage = GetPercentage(inputs.Sum(input => input.Timing));
        }
        private float GetPercentage(int timingSum) => UnityEngine.Mathf.Clamp01((_minMax - timingSum) / _minMax);
    }
}
