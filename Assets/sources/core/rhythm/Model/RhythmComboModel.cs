namespace PataRoad.Core.Rhythm.Command
{
    /// <summary>
    /// Contains combo data.
    /// </summary>
    public class RhythmComboModel
    {
        /// <summary>
        /// How many combo is - n from "n combo".
        /// </summary>
        public int ComboCount { get; }
        /// <summary>
        /// The (recent) command on this combo.
        /// </summary>
        public RhythmCommandModel Command { get; }
        /// <summary>
        /// Determines if it has chance to enter fever.
        /// </summary>
        public bool hasFeverChance { get; }
        internal RhythmComboModel(RhythmCommandModel commandModel, int comboCount)
        {
            Command = commandModel;
            ComboCount = comboCount;
            hasFeverChance = commandModel.ComboType != ComboStatus.NoFever;
        }
    }
}
