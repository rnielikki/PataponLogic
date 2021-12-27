namespace PataRoad.SceneLogic.Minigame
{
    public class MinigameModel
    {
        public MinigameData MinigameData { get; private set; }
        public bool IsPractice { get; private set; }
        public Core.Items.IItem Reward { get; private set; }
        public int RewardAmount { get; private set; }
        public float PerfectionRequiprement { get; private set; } //range of 0-1

        /// <summary>
        /// Generates model to pass PRACTICE data.
        /// </summary>
        /// <param name="minigameData">The minigame to play.</param>
        public MinigameModel(MinigameData minigameData)
        {
            MinigameData = minigameData;
        }
        /// <summary>
        /// Generates model to pass NON-PRACTICE data.
        /// </summary>
        /// <param name="minigameData">The minigame to play.</param>
        /// <param name="perfectionRequirement">0-1 range of perfection requirement, depends on the material.</param>
        /// <param name="reward">Reward item.</param>
        /// <param name="itemAmount">Amount of the item that will get as reward. Default is 1.</param>
        public MinigameModel(MinigameData minigameData, float perfectionRequirement, Core.Items.IItem reward, int itemAmount = 1)
        {
            PerfectionRequiprement = perfectionRequirement;
            MinigameData = minigameData;
            Reward = reward;
            RewardAmount = itemAmount;
        }
    }
}
