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
        /// Combo sequenc, which can determine "may enter fever" status.
        /// </summary>
        public int SequenceCount { get; }
        /// <summary>
        /// The (recent) command on this combo.
        /// </summary>
        public RhythmCommandModel Command { get; }
        /// <summary>
        /// Determines if it has chance to enter fever.
        /// </summary>
        public bool hasFeverChance { get; }
        internal RhythmComboModel(RhythmCommandModel commandModel, int comboCount, int sequcneCount)
        {
            Command = commandModel;
            SequenceCount = sequcneCount;
            ComboCount = comboCount;
            hasFeverChance = commandModel.ComboType != ComboStatus.NoFever;
        }
    }
}
